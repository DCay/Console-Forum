package org.sanity.consoleForum.commands;

import org.sanity.consoleForum.commands.Attributes.AdminCommand;
import org.sanity.consoleForum.commands.Attributes.CommandExample;
import org.sanity.consoleForum.commands.Attributes.CommandHelper;
import org.sanity.consoleForum.commands.Attributes.ModeratorCommand;
import org.sanity.consoleForum.common.Constants;
import org.sanity.consoleForum.core.Principal;
import org.sanity.consoleForum.database.EntityManager;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.Comment;

import java.io.IOException;
import java.util.List;

@ModeratorCommand
@AdminCommand
@CommandHelper(help = "Edits a Comment by given Id, updating its content.")
@CommandExample(example = "EditComment|1|New Content")
public class EditCommentCommand implements Command {
    private static final String NoSuchCommentMessage = "There is no Comment with the given Id.";

    private static final String SuccessEditMessage = "Successfully Editted Comment - %s!";

    private InputReader consoleReader;

    private OutputWriter consoleWriter;

    private List<String> parameters;

    private Principal principal;

    private boolean resetsConsole;

    private EntityManager entityManager;

    @Override
    public InputReader getConsoleReader() {
        return this.consoleReader;
    }

    @Override
    public void setConsoleReader(InputReader inputReader) {
        this.consoleReader = inputReader;
    }

    @Override
    public OutputWriter getConsoleWriter() {
        return this.consoleWriter;
    }

    @Override
    public void setConsoleWriter(OutputWriter outputWriter) {
        this.consoleWriter = outputWriter;
    }

    @Override
    public List<String> getParameters() {
        return this.parameters;
    }

    @Override
    public void setParameters(List<String> parameters) {
        this.parameters = parameters;
    }

    @Override
    public Principal getPrincipal() {
        return this.principal;
    }

    @Override
    public void setPrincipal(Principal principal) {
        this.principal = principal;
    }

    @Override
    public boolean getResetsConsole() {
        return this.resetsConsole;
    }

    @Override
    public void setResetsConsole(boolean resetsConsole) {
        this.resetsConsole = resetsConsole;
    }

    @Override
    public EntityManager getEntityManager() {
        return entityManager;
    }

    @Override
    public void setEntityManager(EntityManager entityManager) {
        this.entityManager = entityManager;
    }

    public void execute() throws IOException, InterruptedException {
        int commentId = Integer.parseInt(this.getParameters().get(0));
        String newContent = this.getParameters().get(1);

        Comment commentFromDb = this.getEntityManager().getComments()
                .stream()
                .filter(comment -> comment.getId() == commentId)
                .findFirst()
                .orElse(null);

        if (commentFromDb == null || commentFromDb.getIsDeleted()) {
            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.errorLine(NoSuchCommentMessage);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        } else {
            commentFromDb.setContent(newContent);

            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.successLine(SuccessEditMessage, commentId);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        }
    }
}
