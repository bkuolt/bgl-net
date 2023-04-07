using System.Windows.Controls;

/// /////////////////


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

            //  throw new System.Exception();
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
            item.Header = mesh.Name.Length > 0 ? mesh.Name : string.Format("Mesh #{0}", index);
            parent.Items.Add(item);


            string text = string.Format("Vertices: {0}\nIndices: {1}\nNormals: {2}",
                                        mesh.VertexCount, mesh.GetIndices().Length, mesh.Normals.Count);
            var label = new Label();
            label.Content = text;
            item.Items.Add(label);
        }


    }



}