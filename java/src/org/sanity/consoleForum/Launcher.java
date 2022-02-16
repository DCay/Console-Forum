package org.sanity.consoleForum;

import org.sanity.consoleForum.commands.AboutCommand; // READY
import org.sanity.consoleForum.commands.AllPostsCommand; // READY
import org.sanity.consoleForum.commands.ClearCommand; // READY
import org.sanity.consoleForum.commands.CommentCommand; // READY
import org.sanity.consoleForum.commands.CreatePostCommand; // READY
import org.sanity.consoleForum.commands.DeleteCommentCommand; // READY
import org.sanity.consoleForum.commands.DeletePostCommand; // READY
import org.sanity.consoleForum.commands.DemoteCommand; // READY
import org.sanity.consoleForum.commands.PromoteCommand; // READY
import org.sanity.consoleForum.commands.DislikeCommand; // READY
import org.sanity.consoleForum.commands.LikeCommand; // READY
import org.sanity.consoleForum.commands.EditPostCommand; // READY
import org.sanity.consoleForum.commands.EditCommentCommand; // READY
import org.sanity.consoleForum.commands.HelpCommand;
import org.sanity.consoleForum.commands.LoginCommand; // READY
import org.sanity.consoleForum.commands.LogoutCommand; // READY
import org.sanity.consoleForum.commands.RegisterCommand; // READY
import org.sanity.consoleForum.commands.ReadCommand; // READY
import org.sanity.consoleForum.commands.ExitCommand; // READY
import org.sanity.consoleForum.core.CommandManager;
import org.sanity.consoleForum.core.ConsoleForumCommandManager;
import org.sanity.consoleForum.core.ConsoleForumEngine;
import org.sanity.consoleForum.core.Engine;
import org.sanity.consoleForum.database.EntityManager;
import org.sanity.consoleForum.io.ConsoleReader;
import org.sanity.consoleForum.io.ConsoleWriter;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.Post;
import org.sanity.consoleForum.models.User;

import java.util.List;
import java.util.Map;

public class Launcher {
    public static void main(String[] args) {
	InputReader inputReader = new ConsoleReader();
        OutputWriter outputWriter = new ConsoleWriter();

        EntityManager entityManager = new EntityManager();
        CommandManager commandManager = new ConsoleForumCommandManager(inputReader, outputWriter, entityManager);

        Engine engine = new ConsoleForumEngine(commandManager, entityManager);
        engine.run();
    }
}
