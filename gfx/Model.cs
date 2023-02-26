
namespace bgl
{
    public class Model
    {
        public void Draw()
        {
            foreach (var mesh in meshes)
            {
                mesh.Draw();
            }
        }

        private Mesh[] meshes;
    };

}
