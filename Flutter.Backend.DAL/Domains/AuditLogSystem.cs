using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public virtual void SetCreatedInFo(string actionUserId , string actionUserName )
        {
            CreatedByName = actionUserName;
            if(ObjectId.TryParse(actionUserId, out ObjectId Userid))
            {
                CreatedByID = Userid;
            }
            CreatedByTime = DateTime.Now;
        }

        public virtual void SetUpdatedInFo(string actionUserId , string actionUserName)
        {
            UpdatedByName = actionUserName;
            if (ObjectId.TryParse(actionUserId, out ObjectId Userid))
            {
                UpdatedByID = Userid;
            }
            UpdatedByTime = DateTime.Now;
        }

        public virtual void SetFullInfo(string actionUserId, string actionUserName)
        {
            SetCreatedInFo(actionUserId, actionUserName);
            SetUpdatedInFo(actionUserId , actionUserName);
        }

    }
}
