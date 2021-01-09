using System;
using System.Collections.Generic;
using System.Text.Json;
using ProjectServer.Models;
using ProjectServer.Services;
using SharedProject;
using SharedProject.DTO;

namespace ProjectServer.Handlers
{
    internal partial class WebSocketHandler  
    {
        private void HandleCreateTopic()
        {
            throw new NotImplementedException();
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
                var resp = new ServerSimpleResponse<InfoDto>("error", new InfoDto{ Message = "Error while fetching data" });
                Communication.SendResponse<ServerSimpleResponse<InfoDto>, InfoDto>(_webSocket, resp);
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