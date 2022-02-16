package org.sanity.consoleForum.database;

import org.sanity.consoleForum.io.FileReader;
import org.sanity.consoleForum.io.FileWriter;
import org.sanity.consoleForum.io.InputReader;
import org.sanity.consoleForum.io.OutputWriter;
import org.sanity.consoleForum.models.Comment;
import org.sanity.consoleForum.models.Post;
import org.sanity.consoleForum.models.PostRating;
import org.sanity.consoleForum.models.User;
import org.sanity.consoleForum.models.enums.PostRatingChoice;
import org.sanity.consoleForum.models.enums.UserRole;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardOpenOption;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class EntityManager {
    protected List<User> users = new ArrayList<>();

    protected List<Post> posts =new ArrayList<>();

    protected List<Comment> comments =new ArrayList<>();

    protected List<PostRating> ratings =new ArrayList<>();

    public List<User> getUsers() {
        return users;
    }

    public List<Post> getPosts() {
        return posts;
    }

    public List<Comment> getComments() {
        return comments;
    }

    public List<PostRating> getRatings() {
        return ratings;
    }

    private void clearFiles() throws IOException {
        String dbDirectoryPath = System.getProperty("user.dir") + "/org/sanity/consoleForum/db/";

        Files.delete(Path.of(dbDirectoryPath + "users.db"));
        Files.delete(Path.of(dbDirectoryPath + "posts.db"));
        Files.delete(Path.of(dbDirectoryPath + "comments.db"));
        Files.delete(Path.of(dbDirectoryPath + "ratings.db"));
    }

    private void initializeUsers(List<String> unparsedUsers) {
        for (var unparsedUser : unparsedUsers) {
            String[] unparsedParams = unparsedUser.split(",");

            int id = Integer.parseInt(unparsedParams[0]);
            String username = unparsedParams[1];
            String password = unparsedParams[2];
            boolean isEnabled = Boolean.parseBoolean(unparsedParams[3]);
            UserRole role = UserRole.valueOf(unparsedParams[4].toUpperCase());
            boolean isDeleted = Boolean.parseBoolean(unparsedParams[5]);

            if (isDeleted) continue;

            this.users.add(new User() {{
                setId(id);
                setUsername(username);
                setPassword(password);
                setIsEnabled(isEnabled);
                setRole(role);
            }});
        }
    }

    private void initializePosts(List<String> unparsedPosts) {
        for (var unparsedPost : unparsedPosts)
        {
            String[] unparsedParams = unparsedPost.split(",");

            int id = Integer.parseInt(unparsedParams[0]);
            String title = unparsedParams[1];
            String content = unparsedParams[2];
            User postUser = users.stream()
                    .filter(user -> user.getId() == Integer.parseInt(unparsedParams[3]))
                    .findFirst()
                    .orElse(null);
            boolean isDeleted = Boolean.parseBoolean(unparsedParams[4]);

            if (isDeleted) continue;

            Post postForDb = new Post() {{
                setId(id);
                setTitle(title);
                setContent(content);
                setUser(postUser);
            }};

            posts.add(postForDb);
            postUser.getPosts().add(postForDb);
        }

    }

    private void initializeComments(List<String> unparsedComments) {
        for (var unparsedComment : unparsedComments)
        {
            String[] unparsedParams = unparsedComment.split(",");

            int id = Integer.parseInt(unparsedParams[0]);
            String content = unparsedParams[1];
            User commentUser = users.stream().filter(user -> user.getId() == Integer.parseInt(unparsedParams[2])).findFirst().orElse(null);
            Post commentPost = posts.stream().filter(post -> post.getId() == Integer.parseInt(unparsedParams[3])).findFirst().orElse(null);
            boolean isDeleted = Boolean.parseBoolean(unparsedParams[4]);

            if (isDeleted) continue;

            Comment commentForDb = new Comment() {{
                setId(id);
                setContent(content);
                setUser(commentUser);
                setPost(commentPost);
            }};

            comments.add(commentForDb);
            commentUser.getComments().add(commentForDb);
            commentPost.getComments().add(commentForDb);
        }

    }

    private void initializeRatings(List<String> unparsedRatings) {
        for (var unparsedRating : unparsedRatings)
        {
            String[] unparsedParams = unparsedRating.split(",");

            int id = Integer.parseInt(unparsedParams[0]);
            PostRatingChoice choice = PostRatingChoice.valueOf(unparsedParams[1].toUpperCase());
            User ratingUser = users.stream().filter(user -> user.getId() == Integer.parseInt(unparsedParams[2])).findFirst().orElse(null);
            Post ratingPost = posts.stream().filter(post -> post.getId() == Integer.parseInt(unparsedParams[3])).findFirst().orElse(null);
            boolean isDeleted = Boolean.parseBoolean(unparsedParams[4]);

            if (isDeleted) continue;

            PostRating ratingForDb = new PostRating(choice) {{
                setId(id);
                setUser(ratingUser);
                setPost(ratingPost);
            }};

            ratings.add(ratingForDb);
            ratingUser.getRatings().add(ratingForDb);
            ratingPost.getRatings().add(ratingForDb);
        }
    }

    public User add(User user) {
        user.setId(this.users.size());
        this.users.add(user);
        return user;
    }

    public Post add(Post post) {
        post.setId(this.posts.size());
        this.posts.add(post);
        return post;
    }

    public Comment add(Comment comment) {
        comment.setId(this.comments.size());
        this.comments.add(comment);
        return comment;
    }

    public PostRating add(PostRating postRating) {
        postRating.setId(this.ratings.size());
        this.ratings.add(postRating);
        return postRating;
    }

    public void initialize() {
        try {
            // READ DATA FROM
            List<String> unparsedUsers = new ArrayList<>();
            List<String> unparsedPosts = new ArrayList<>();
            List<String> unparsedComments = new ArrayList<>();
            List<String> unparsedRatings = new ArrayList<>();

            // READ FROM FILES WITH INPUT READERS
            String dbDirectoryPath = System.getProperty("user.dir") + "/org/sanity/consoleForum/db/";

            InputReader usersReader = new FileReader(dbDirectoryPath + "users.db");
            InputReader postsReader = new FileReader(dbDirectoryPath + "posts.db");
            InputReader commentsReader = new FileReader(dbDirectoryPath + "comments.db");
            InputReader ratingsReader = new FileReader(dbDirectoryPath + "ratings.db");

            String line = null;

            while ((line = usersReader.readLine()) != null) unparsedUsers.add(line);
            while ((line = postsReader.readLine()) != null) unparsedPosts.add(line);
            while ((line = commentsReader.readLine()) != null) unparsedComments.add(line);
            while ((line = ratingsReader.readLine()) != null) unparsedRatings.add(line);
            // ADD LINES TO LISTS OF StringS

            initializeUsers(unparsedUsers);
            initializePosts(unparsedPosts);
            initializeComments(unparsedComments);
            initializeRatings(unparsedRatings);
        } catch (Exception e) { e.printStackTrace(); }
    }

    public void store() throws IOException {
        String dbDirectoryPath = System.getProperty("user.dir") + "/org/sanity/consoleForum/db/";

        this.clearFiles();

        if(!Files.exists(Path.of(dbDirectoryPath))) Files.createDirectory(Path.of(dbDirectoryPath));
        if(!Files.exists(Path.of(dbDirectoryPath + "users.db"))) Files.createFile(Path.of(dbDirectoryPath + "users.db"));
        if(!Files.exists(Path.of(dbDirectoryPath + "posts.db"))) Files.createFile(Path.of(dbDirectoryPath + "posts.db"));
        if(!Files.exists(Path.of(dbDirectoryPath + "comments.db"))) Files.createFile(Path.of(dbDirectoryPath + "comments.db"));
        if(!Files.exists(Path.of(dbDirectoryPath + "ratings.db"))) Files.createFile(Path.of(dbDirectoryPath + "ratings.db"));

        OutputWriter usersWriter = new FileWriter(dbDirectoryPath + "users.db", StandardOpenOption.APPEND);
        OutputWriter postsWriter = new FileWriter(dbDirectoryPath + "posts.db", StandardOpenOption.APPEND);
        OutputWriter commentsWriter = new FileWriter(dbDirectoryPath + "comments.db", StandardOpenOption.APPEND);
        OutputWriter ratingsWriter = new FileWriter(dbDirectoryPath + "ratings.db", StandardOpenOption.APPEND);

        for (User user : users) usersWriter.write(user);
        for (Post post : posts) postsWriter.write(post);
        for (Comment comment : comments) commentsWriter.write(comment);
        for (PostRating rating : ratings) ratingsWriter.write(rating);
    }
}
