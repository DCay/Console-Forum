namespace ConsoleForum.App.Common
{
    public static class Constants
    {
        public const string ConsoleForumIntroBanner = @"#####################################################################
##                                                                 ##
##         #####  #####  #   #  #####  #####  #      #####         ##
##         #      #   #  ##  #  #      #   #  #      #             ##
##         #      #   #  # # #  #####  #   #  #      #####         ##
##         #      #   #  #  ##      #  #   #  #      #             ##
##         #####  #####  #   #  #####  #####  #####  #####         ##
##                                                                 ##
##                #####  #####  #####  #   #  #   #                ##
##                #      #   #  #   #  #   #  ## ##                ##
##                #####  #   #  #####  #   #  # # #                ##
##                #      #   #  # #    #   #  #   #                ##
##                #      #####  #  ##   ###   #   #                ##
##                                                                 ##
#####################################################################";

        public const string ConsoleForumAbout = @"#####################################################################
##                                                                 ##
## Welcome to the Console Forum!                                   ##
##                                                                 ##
## This is a simple Forum application.                             ##
##                                                                 ##
## Guests can Login or Register.                                   ##
##                                                                 ##
## Logged-In Users can create Posts.                               ##
## Logged-In Users can create Comments on other Posts.             ##
## Logged-In Users can Rate Posts by Liking or Disliking them.     ##
##                                                                 ##
## The Console Forum also has Moderation and Administration.       ##
## Logged-In Moderators can do everything a normal User can do.    ##
## Logged-In Moderators can Modify or Delete Posts and Comments.   ##
## Logged-In Administartors can do everything a Moderator can do.  ##
## Logged-In Administrators can Promote or Demote Users.           ##
##                                                                 ##
## Type in ""help"" if you need help with the commands.              ##
## Console Forum wishes you a happy experience! :)                 ##
##                                                                 ##
#####################################################################";

        public const string AdminIntroductionMessage = @"## Greetings, Administrator {0}!{1} ##
##                                                                 ##
## Type in ""help"" to view the available commands.                  ##
##                                                                 ##
#####################################################################";

        public const string ModeratorIntroductionMessage = @"## Welcome, Moderator {0}!{1} ##
## Keep up the good work! :)                                       ##
##                                                                 ##
## Type in ""help"" to view the available commands.                  ##
##                                                                 ##
#####################################################################";

        public const string UserIntroductionMessage = @"## Welcome, {0}!{1} ## 
## Console Forum wishes you a happy experience! :)                 ##
##                                                                 ##
## Type in ""help"" to view the available commands.                  ##
##                                                                 ##
#####################################################################";

        public const string GuestIntroductionMessage = @"## If you are not familiar with the application, type in ""about""!  ##
## This will give you a detailed summary of the Console Forum.     ##
##                                                                 ##
## You can also type in ""help"" to view the available commands.     ##
##                                                                 ##
## Console Forum wishes you a happy experience! :)                 ##
##                                                                 ##
#####################################################################";

        public const string ConsoleForumOutputEmptyLine = "##                                                                 ##";

        public const string ConsoleForumOutputPrefix = "## ";

        public const int NotificationDelay = 1500;

        public const int ExitNotificationDelay = 2500;
    }
}
