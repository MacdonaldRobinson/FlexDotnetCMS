using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public class TwitterHelper
    {
        public TwitterHelper(string twitterApiConsumerKey, string twitterApiConsumerSecret, string twitterApiAccessToken, string twitterApiAccessTokenSecret)
        {
            TwitterApiConsumerKey = twitterApiConsumerKey;
            TwitterApiConsumerSecret = twitterApiConsumerSecret;
            TwitterApiAccessToken = twitterApiAccessToken;
            TwitterApiAccessTokenSecret = twitterApiAccessTokenSecret;
        }

        public string TwitterApiConsumerKey { get; private set; }

        public string TwitterApiConsumerSecret { get; private set; }

        public string TwitterApiAccessToken { get; private set; }

        public string TwitterApiAccessTokenSecret { get; private set; }

        public IAuthorizer GetOAuthToken()
        {
            return new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = TwitterApiConsumerKey,
                    ConsumerSecret = TwitterApiConsumerSecret,
                    AccessToken = TwitterApiAccessToken,
                    AccessTokenSecret = TwitterApiAccessTokenSecret
                }
            };
        }

        public dynamic GetTweetsByScreenName(string screenName, int count = 2, bool returnRawResults = false)
        {
            var key = "HomeTweets";
            var rawResultsKey = "RawResultHomeTweets";

            var homeTweets = (List<LinqToTwitter.Status>)ContextHelper.GetFromCache(key);
            var rawResults = (string)ContextHelper.GetFromCache(rawResultsKey);

            if ((returnRawResults) && (!string.IsNullOrEmpty(rawResults)))
                return rawResults;

            if (homeTweets != null)
                return homeTweets;

            try
            {
                var twitterContext = new TwitterContext(GetOAuthToken());

                var queryResults =
                    (from tweet in twitterContext.Status
                     where tweet.Type == StatusType.User && tweet.ScreenName == screenName && tweet.Count == count && tweet.IncludeRetweets == true
                     select tweet);

                var queryResultsList = queryResults.ToList();

                ContextHelper.SaveToCache(key, queryResultsList, DateTime.Now.AddMinutes(10));
                ContextHelper.SaveToCache(rawResultsKey, twitterContext.RawResult, DateTime.Now.AddMinutes(10));

                if (returnRawResults)
                    return twitterContext.RawResult;
                else
                    return queryResultsList;
            }
            catch (Exception ex)
            {
                ErrorHelper.LogException(ex);
                ContextHelper.RemoveFromCache(key);
            }

            return homeTweets;
        }
    }
}