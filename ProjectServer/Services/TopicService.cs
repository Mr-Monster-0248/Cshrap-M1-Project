using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectServer.Models;
using SharedProject;
using SharedProject.DTO;

namespace ProjectServer.Services
{
    public class TopicService
    {
        /// <summary>
        /// Get all the topics available
        /// </summary>
        /// <returns>A list of Topic object</returns>
        public static List<Topic> GetTopics()
        {
            var context = DbServices.Instance.Context;
            return context.Topics.ToList();
            
        }

        public static List<Topic> GetTopics(string name)
        {
            var context = DbServices.Instance.Context;
            return context.Topics.Where(topic => topic.Title.Contains(name)).ToList();
        }

        public static List<Topic> GetConnectedTopics(int userId)
        {
            var context = DbServices.Instance.Context;

            return (from t in context.Topics 
                    join c in context.Connects 
                        on t.TopicsId equals c.TopicsId 
                    where c.UserId == userId 
                    select t
                    ).ToList();
        }

        public static bool AddTopic(Topic newTopic)
        {
            var context = DbServices.Instance.Context;

            try
            {
                context.Topics.Add(newTopic);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}