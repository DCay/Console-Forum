package org.sanity.consoleForum.commands;

import org.sanity.consoleForum.commands.Attributes.*;
import org.sanity.consoleForum.common.Constants;
import org.sanity.consoleForum.common.Validator;
import org.sanity.consoleForum.core.Principal;
import org.sanity.consoleForum.database.EntityManager;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.Comment;
import org.sanity.consoleForum.models.Post;

import java.io.IOException;
import java.util.List;

@UserCommand
@ModeratorCommand
@AdminCommand
@CommandHelper(help = "Creates a Comment to a Post, by given Post Id.")
@CommandExample(example = "Comment|1|This is a Simple Comment.")
public class CommentCommand implements Command {
    private static final String NoSuchPostMessage = "There is no Post with the given Id.";

    private static final String ContentMalformed = "Content must not be empty.";

    private static final String SuccessCreateCommentMessage = "Successfully created Comment for Post - %s!";

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
        return this.entityManager;
    }

    @Override
    public void setEntityManager(EntityManager entityManager) {
        this.entityManager = entityManager;
    }

    @Override
    public void execute() throws IOException, InterruptedException {
        int postId = Integer.parseInt(this.getParameters().get(0));
        String content = this.getParameters().get(1);

        Post postFromDb = this.getEntityManager().getPosts()
                .stream()
                .filter(post -> post.getId() == postId)
                .findFirst()
                .orElse(null);

        if (postFromDb == null || postFromDb.getIsDeleted())
        {
            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.errorLine(NoSuchPostMessage);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        }
        else if (Validator.isNullOrEmpty(content))
        {
            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.errorLine(ContentMalformed);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        }
        else
        {
            Comment commentForDb = new Comment() {{
                setContent(content);
                setUser(getPrincipal().getUser());
                setPost(postFromDb);
            }};

            this.getEntityManager().add(commentForDb);
            this.getPrincipal().getUser().getComments().add(commentForDb);
            postFromDb.getComments().add(commentForDb);

            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.successLine(SuccessCreateCommentMessage, postId);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        }
    }
}
