using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contoso.Apps.Movies.Data.Models
{
    [Serializable]
    public class Eruption
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int EruptionId { get; set; }

        /// <summary>
        /// Identifies the Event Hub region to which this message was sent.
        /// </summary>
        public string Region { get; set; }

        public System.DateTime OrderDate { get; set; }

        public string VolcanoName { get; set; }
    }
}
