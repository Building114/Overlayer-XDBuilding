using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Overlayer.Core.TextReplacing.Parsing;

public class ParsedTag : IParsed {
    public Tag tag;
    public List<string> args;
    public List<Tag> extraReferences;
    public IEnumerable<Tag> ReferencedTags => extraReferences == null ? new[] { tag } : new[] { tag }.Concat(extraReferences);
    public ParsedTag(Tag tag, List<string> args, List<Tag> extraReferences = null) {
        this.tag = tag;
        this.args = args;
        this.extraReferences = extraReferences ?? new List<Tag>();
    }
    public void Emit(ILGenerator il) {
        var parameters = tag.GetterOriginal.GetParameters();
        for(int i = 0; i < tag.ArgumentCount; i++) {
            if(args.Count - 1 < i) {
                il.Emit(OpCodes.Ldstr, parameters[i].DefaultValue?.ToString() ?? string.Empty);
            } else {
                il.Emit(OpCodes.Ldstr, args[i]);
            }
        }
        il.Emit(OpCodes.Call, tag.Getter);
    }
}
