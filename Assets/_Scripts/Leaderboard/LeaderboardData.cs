using System;
using System.Collections.Generic;

namespace Firestore
{
    [Serializable]
    public class LeaderboardData
    {
        public List<LeaderboardEntry> documents;
        
        [Serializable]
        public struct LeaderboardEntry
        {
            public Fields fields;
            public string Name => fields.name.stringValue;
            public int Score => int.Parse(fields.score.integerValue);

            [Serializable]
            public struct Fields {
                public NameValue name;
                public ScoreValue score;
            }
            [Serializable]
            public struct NameValue {
                public string stringValue;
            }
            [Serializable]
            public struct ScoreValue {
                public string integerValue;
            }
        }
    }
}