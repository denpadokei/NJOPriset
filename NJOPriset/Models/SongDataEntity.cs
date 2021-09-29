using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJOPriset.Models
{
    public record SongDataEntity
    {
        public string LevelId { get; set; }
        public string ParentDiff { get; set; }
        public int Diff { get; set; }
        public SongDataEntity()
        {
            this.LevelId = "";
            this.ParentDiff = "";
            this.Diff = 0;
        }
        public SongDataEntity(string levelId, string parentDiff, int diff)
        {
            this.LevelId = levelId;
            this.ParentDiff = parentDiff;
            this.Diff = diff;
        }
    }
}
