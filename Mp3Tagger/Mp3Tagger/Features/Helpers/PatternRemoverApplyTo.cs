using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Tagger.Features.Helpers
{
    public class PatternRemoverApplyTo
    {
        public string FieldName { get; set; }
        public bool IsApply { get; set; }

        public PatternRemoverApplyTo()
        {
            FieldName = "name";
            IsApply = false;
        }
    }
}
