using MongoDB.Bson;
using System;

namespace Flutter.Backend.DAL.Domains
{
    public class AuditLogSystem
    {

        public ObjectId Id { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedByTime { get; set; }

        public ObjectId CreatedByID { get; set; }

        public string UpdatedByName { get; set; }

        public DateTime UpdatedByTime { get; set; }

        public ObjectId UpdatedByID { get; set; }


        public virtual void SetCreatedInFor(string actionUserId , string actionUserName )
        {
            CreatedByName = actionUserName;
            if(ObjectId.TryParse(actionUserId, out ObjectId Userid))
            {
                CreatedByID = Userid;
            }
            CreatedByTime = DateTime.Now;
        }

        public virtual void SetUpdatedInFor(string actionUserId , string actionUserName)
        {
            UpdatedByName = actionUserName;
            if (ObjectId.TryParse(actionUserId, out ObjectId Userid))
            {
                UpdatedByID = Userid;
            }
            UpdatedByTime = DateTime.Now;
        }

        public virtual void SetFullInfor(string actionUserId, string actionUserName)
        {
            SetCreatedInFor(actionUserId, actionUserName);
            SetUpdatedInFor(actionUserId , actionUserName);
        }

    }
}
