using System.CodeDom;

namespace System.Web.UI
{
    //从mvc2_ms_pl中提取
    internal sealed class ViewPageControlBuilder : FileLevelPageControlBuilder
    {
        public string PageBaseType { get; set; }

        public override void ProcessGeneratedCode(
            CodeCompileUnit codeCompileUnit,
            CodeTypeDeclaration baseType,
            CodeTypeDeclaration derivedType,
            CodeMemberMethod buildMethod,
            CodeMemberMethod dataBindingMethod)
        {

            // If we find got a base class string, use it
            if (PageBaseType != null)
            {
                derivedType.BaseTypes[0] = new CodeTypeReference(PageBaseType);
            }
        }
    }
}
