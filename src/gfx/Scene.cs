namespace bgl
{
    public class Scene
    {
#if false
        public class Node
        {
            public readonly Node? Parent;
            public Node[]? Children;
            public Matrix4D? Transform;

            public bgl.Graphics.Core.Mesh[]? Meshes;
            public Camera? Camera;

            public Node(in Node parent)
            {
                this.Parent = parent;
            }

            public void Draw(in Matrix4D viewMatrix)
            {
                foreach (bgl.Graphics.Core.Mesh mesh in Meshes)
                {
                    var worldMatrix = Parent.GetWorldTransform() * Transform;
                    mesh.Draw(worldMatrix, viewMatrix);
                }
            }

            private Matrix4D GetWorldTransform()
            {
                return new Matrix4D(); // TODO
            }
        }

        public Node Root;

        public void Draw()
        {
            Matrix4D worldMatrix = new Matrix4D();

            var cameraNode = cameras[currentCamera];
            Matrix4D viewMatrix = cameraNode.Camera.ViewMatrix;

            foreach (Node node in GetAllNodes())
            {
                viewMatrix = viewMatrix * node.Parent.Transform;
                foreach (bgl.Graphics.Core.Mesh mesh in node.Meshes)
                {
                    mesh.Draw(worldMatrix, viewMatrix);
                }
            }
        }


        public void SetCamera(uint index)
        {
            // TODO
        }

        private Node[] GetAllNodes()
        {
            return null; // TODO
        }

        private Node[] cameras;
        private uint currentCamera;
#endif 
        public void Pick(OpenTK.Mathematics.Vector2 mouseCoordinates, OpenTK.Mathematics.Vector2 windowBounds)
        {
            // TODO
        }

    }

}
