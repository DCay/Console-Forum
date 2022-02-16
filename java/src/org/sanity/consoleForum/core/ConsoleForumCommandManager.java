package org.sanity.consoleForum.core;

import org.sanity.consoleForum.commands.Attributes.AdminCommand;
import org.sanity.consoleForum.commands.Attributes.GuestCommand;
import org.sanity.consoleForum.commands.Attributes.ModeratorCommand;
import org.sanity.consoleForum.commands.Attributes.UserCommand;
import org.sanity.consoleForum.commands.Command;
import org.sanity.consoleForum.common.Constants;
import org.sanity.consoleForum.database.EntityManager;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.enums.UserRole;

import java.io.IOException;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class ConsoleForumCommandManager implements CommandManager {
    private final InputReader consoleReader;

    private final OutputWriter consoleWriter;

    private final EntityManager entityManager;

    private Principal currentlyLoggedInUser;

    public ConsoleForumCommandManager(InputReader consoleReader, OutputWriter consoleWriter, EntityManager entityManager) {
        this.consoleReader = consoleReader;
        this.consoleWriter = consoleWriter;
        this.entityManager = entityManager;
        this.currentlyLoggedInUser = new PrincipalImpl();
    }

    // TODO: METHODS TO CHECK IF THE CURRENT USER IS LOGGED IN AND IS ADMIN OR MODERATOR
    private boolean isLoggedIn() {
        return false; // TODO: IMPLEMENT ME ...
    }

    private boolean isAdmin() {
        return false; // TODO: IMPLEMENT ME ...
    }

    private boolean isModerator() {
        return false; // TODO: IMPLEMENT ME ...
    }

    private String getAppropriateHeader() {
        if (this.isAdmin()) return String.format(Constants.ADMIN_INTRODUCTION_MESSAGE, this.currentlyLoggedInUser.getUser().getUsername(), " ".repeat(37 - this.currentlyLoggedInUser.getUser().getUsername().length()));
        if (this.isModerator()) return String.format(Constants.MODERATOR_INTRODUCTION_MESSAGE, this.currentlyLoggedInUser.getUser().getUsername(), " ".repeat(43 - this.currentlyLoggedInUser.getUser().getUsername().length()));
        if (this.isLoggedIn()) return String.format(Constants.USER_INTRODUCTION_MESSAGE, this.currentlyLoggedInUser.getUser().getUsername(), " ".repeat(53 - this.currentlyLoggedInUser.getUser().getUsername().length()));
        return Constants.GUEST_INTRODUCTION_MESSAGE;
    }

    private void clearConsole() {
        try {
            if (System.getProperty("os.name").contains("Windows"))
                new ProcessBuilder("cmd", "/c", "cls").inheritIO().start().waitFor();
            else
                Runtime.getRuntime().exec("clear");
        } catch (IOException | InterruptedException ignored) { }
    }

    private void printIntro() {
        try {
            this.consoleWriter.writeLine(Constants.CONSOLE_FORUM_INTRO_BANNER);
            this.consoleWriter.writeLine(Constants.CONSOLE_FORUM_OUTPUT_EMPTY_LINE);
            this.consoleWriter.writeLine(this.getAppropriateHeader());
            this.consoleWriter.writeLine(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    // THIS CREATES A COMMAND OBJECT FROM A CLASS BASED ON INPUT
    private Command instanceCommand(String input) throws ClassNotFoundException, NoClassDefFoundError, NoSuchMethodException, InvocationTargetException, InstantiationException, IllegalAccessException {
        List<String> inputParams = Arrays.stream(input.split("\\|")).collect(Collectors.toList());
        String commandName = inputParams.get(0);
        inputParams.remove(0);

        Class commandType = null;

        commandType = Class.forName("org.sanity.consoleForum.commands." + commandName + "Command");

        if (commandType != null) {
            Command command = null;

            command = (Command) commandType.getDeclaredConstructor().newInstance();
            command.setConsoleReader(this.consoleReader);
            command.setConsoleWriter(this.consoleWriter);
            command.setParameters(inputParams);
            command.setPrincipal(this.currentlyLoggedInUser);
            command.setEntityManager(this.entityManager);

            return command;
        }

        return null;
    }

    private boolean isAuthorized(Command command) {
        Class<?> commandClass = command.getClass();

        if (
                (this.currentlyLoggedInUser.getUser() == null && !commandClass.isAnnotationPresent(GuestCommand.class)) ||
                        (this.currentlyLoggedInUser.getUser() != null && this.currentlyLoggedInUser.getUser().getRole() == UserRole.USER
                                && !commandClass.isAnnotationPresent(UserCommand.class)) ||
                        (this.currentlyLoggedInUser.getUser() != null && this.currentlyLoggedInUser.getUser().getRole() == UserRole.MODERATOR
                                && !commandClass.isAnnotationPresent(ModeratorCommand.class)) ||
                        (this.currentlyLoggedInUser.getUser() != null && this.currentlyLoggedInUser.getUser().getRole() == UserRole.ADMIN
                                && !commandClass.isAnnotationPresent(AdminCommand.class))) {
            return false;
        }

        return true;
    }

    public void reset() {
        this.clearConsole();
        this.printIntro();
    }

    public void handleInput() throws IOException, NoClassDefFoundError, InterruptedException, ClassNotFoundException, InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException {
        this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
        String input = this.consoleReader.readLine();

        Command command = this.instanceCommand(input);

        if (command == null) {
            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.errorLine("Unsupported command...", null);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
            this.reset();
        } else {
            if (!this.isAuthorized(command)) {
                this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
                this.consoleWriter.errorLine("You have no access to this command.", null);
                Thread.sleep(Constants.NOTIFICATION_DELAY);
                this.reset();
            } else {
                command.execute();
                if (command.getResetsConsole()) this.reset();
            }
        }
    }
}
