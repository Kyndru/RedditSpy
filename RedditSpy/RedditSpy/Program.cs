using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;
using System.Threading;

namespace RedditSpy
{
    class Program
    {
        static void Main(string[] args)
        {
            var reddit = new Reddit();

            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            Console.Clear();

            var user = reddit.LogIn(username, password);
            var subreddit = reddit.GetSubreddit("/r/leagueoflegends");
            subreddit.Subscribe();
            int iVotes = 0;
            int iKommentare = 0;
            List<string> lstPosts = new List<string>();
            while (true)
            {
                bool bAddLine = false;
                List<Post> userpost = user.GetPosts().ToList();
                
                if (iVotes != (userpost[0].Upvotes - userpost[0].Downvotes))
                {
                    iVotes = (userpost[0].Upvotes - userpost[0].Downvotes);
                    
                    Console.ResetColor();
                    Console.Write(DateTime.Now.ToString() + " - Votes: ");
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Write(iVotes);
                    Console.ResetColor();
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write(userpost[0].Upvotes);
                    Console.ResetColor();
                    Console.Write("  ");
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(userpost[0].Downvotes);
                    Console.ResetColor();
                    Console.Write("  Comments: ");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write(userpost[0].CommentCount);
                    bAddLine = true;
                }
                if (iKommentare < userpost[0].CommentCount)
                {
                    Console.ResetColor();
                    Console.Write("  ");
                    iKommentare = userpost[0].CommentCount;
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("New Comment!");
                    bAddLine = true;
                }

                if (bAddLine)
                    Console.WriteLine(string.Empty);

                var posts = subreddit.GetNew();
                foreach (var post in posts.Take(25))
                {
                    if ((post.Title.ToLower().Contains("halloween") || post.Title.ToLower().Contains("harrowing") || post.Title.ToLower().Contains("skins")) && !lstPosts.Contains(post.Id))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(DateTime.Now.ToString() + " - " + post.Title);

                        lstPosts.Add(post.Id);
                    }
                }

                Thread.Sleep(5000);
            }
        }
    }
}
