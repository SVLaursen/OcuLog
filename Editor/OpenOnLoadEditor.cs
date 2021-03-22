using UnityEditor;

namespace oculog.editor
{
    [InitializeOnLoad]
    public class OpenOnLoadEditor
    {
        static OpenOnLoadEditor()
        {
            EditorApplication.update += StartUp;
        }

        static void StartUp()
        {
            EditorApplication.update -= StartUp;
            
            //All actions go here
            OculogEditor.OpenOnLoad();
        }
    }
}