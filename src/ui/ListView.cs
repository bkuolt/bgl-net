using System.Windows.Controls;


namespace bgl
{

    public class ListView : System.Windows.Controls.TreeView
    {
        readonly TreeViewItem _parent;


        public ListView()
        {
            _parent = new TreeViewItem();
            this.Items.Add(_parent);
            _parent.Header = "Meshes";

            this.Width = 300;
            this.Height = 300;
        }

        public void SetScene(Assimp.Scene scene)
        {
            _parent.Items.Clear();
            AddMeshes(scene);
        }

        private void AddMeshes(Assimp.Scene scene)
        {
            System.Console.WriteLine(scene.MeshCount + "meshes ");

            for (int i = 0; i < scene.MeshCount; ++i)
            {
                AddMesh(scene.Meshes[i], i, _parent);
            }
        }
        private void AddMesh(Assimp.Mesh mesh, int index, TreeViewItem parent)
        {
            TreeViewItem item = new TreeViewItem();
            parent.Items.Add(item);
            item.Header = mesh.Name.Length > 0 ? mesh.Name : string.Format("Mesh #{0}", index);
            item.Items.Add(CreateLabel(mesh));
        }

        private TreeViewItem CreateMaterialLabel(Assimp.Mesh mesh) {  // TODO
            TreeViewItem item =  new TreeViewItem();
            //mesh.MaterialIndex
            return item;
        }

        private Label CreateLabel(Assimp.Mesh mesh) {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Format("Vertices: {0}" ,mesh.VertexCount));
            sb.AppendLine(string.Format("Indices: {0}" ,mesh.GetIndices().Length));
            sb.AppendLine(string.Format("Normals: {0}", mesh.Normals.Count));
            sb.AppendLine(string.Format("Primitive Type: ", mesh.PrimitiveType.ToString()));

            var label = new Label();
            label.Content = sb;
            return label;
        }
    }
}