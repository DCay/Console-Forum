package org.sanity.consoleForum.commands;

import org.sanity.consoleForum.commands.Attributes.*;
import org.sanity.consoleForum.common.Constants;
import org.sanity.consoleForum.core.Principal;
import org.sanity.consoleForum.database.EntityManager;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.Post;
import org.sanity.consoleForum.models.PostRating;
import org.sanity.consoleForum.models.enums.PostRatingChoice;

import java.io.IOException;
import java.util.List;

@UserCommand
@ModeratorCommand
@AdminCommand
@CommandHelper(help = "Dislikes a Post by a given Post Id.")
@CommandExample(example = "Dislike|1")
public class DislikeCommand implements Command {
    private static final String NoSuchPostMessage = "There is no Post with the given Id.";

    private static final String CannotDislikeAgainMessage = "You already disliked this Post.";

    private static final String SuccessDislikeMessage = "Successfully Disliked Post - %s!";

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

    public void execute() throws InterruptedException, IOException {
        int postId = Integer.parseInt(this.getParameters().get(0));

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
        else if (postFromDb.getRatings().stream().anyMatch(rating -> rating.getUser().getId()
                == this.getPrincipal().getUser().getId()))
        {
            PostRating ratingFromDb = postFromDb.getRatings()
                    .stream()
                    .filter(rating -> rating.getUser().getId() == this.getPrincipal().getUser().getId())
                    .findFirst()
                    .orElse(null);

            if (ratingFromDb.getIsNegative())
            {
                this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
                this.consoleWriter.errorLine(CannotDislikeAgainMessage);
                Thread.sleep(Constants.NOTIFICATION_DELAY);
            }
            else
            {
                ratingFromDb.toggle();

                this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
                this.consoleWriter.successLine(SuccessDislikeMessage, postId);
                Thread.sleep(Constants.NOTIFICATION_DELAY);
            }
        }
            else
        {
            PostRating rating = new PostRating(PostRatingChoice.NEGATIVE) {{
                setUser(getPrincipal().getUser());
                setPost(postFromDb);
            }};

            this.getEntityManager().add(rating);

            this.getPrincipal().getUser().getRatings().add(rating);
            postFromDb.getRatings().add(rating);

            this.consoleWriter.write(Constants.CONSOLE_FORUM_OUTPUT_PREFIX);
            this.consoleWriter.successLine(SuccessDislikeMessage, postId);
            Thread.sleep(Constants.NOTIFICATION_DELAY);
        }
    }
}
