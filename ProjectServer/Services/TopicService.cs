using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ProjectServer.Models;

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
            using var context = DbServices.Instance.Context;
            return context.Topics.ToList();
            
        }

        public static List<Topic> GetTopics(string name)
        {
            using var context = DbServices.Instance.Context;
            return context.Topics.Where(topic => topic.Title.Contains(name)).ToList();
        }

        public static List<Topic> GetConnectedTopics(int userId)
        {
            using var context = DbServices.Instance.Context;

            return (from t in context.Topics 
                    join c in context.Connects 
                        on t.TopicsId equals c.TopicsId 
                    where c.UserId == userId 
                    select t
                    ).ToList();
        } 
    }
}