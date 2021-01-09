using System;
using System.Collections.Generic;
using System.Text.Json;
using ProjectServer.Models;
using ProjectServer.Services;
using Serilog;
using SharedProject;
using SharedProject.DTO;

namespace ProjectServer.Handlers
{
    internal partial class WebSocketHandler
    {
        private void HandleCreateTopic(TopicDto newTopic)
        {
            var topic = new Topic {Title = newTopic.Title, Description = newTopic.Description};
            if (TopicService.AddTopic(topic))
            {
                Log.Information($"Topic {topic.Title} created");
                Communication.SendSuccess(_webSocket);
            }
            else
            {
                Communication.SendError(_webSocket, "Wrong username or password");
            }
        }

        private void HandleJoinTopic()
        {
            throw new NotImplementedException();
        }

        private void HandleListTopic()
        {
            var topicList = TopicService.GetTopics();

            if (topicList == null)
            {
                Communication.SendError(_webSocket, "Error while fetching data");
                return;
            }

            var topicListResponse = topicList.ConvertAll(topic =>
            {
                return new TopicDto
                {
                    Id = topic.TopicsId,
                    Title = topic.Title,
                    Description = topic.Description
                };
            });

            var respSuccess = new ServerListResponse<TopicDto>(topicListResponse);
            Communication.SendResponse<ServerListResponse<TopicDto>, TopicDto>(_webSocket, respSuccess);
        }
    }
}