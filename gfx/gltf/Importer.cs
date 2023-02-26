
namespace bgl.gltf {

    public class Importer {

        public Importer(in gltf.Model model) {
            // TODO
        }

/*
        public bgl.Model Import(in string path) {
            //ImportMaterial();
            return new bgl.Model();
        }
*/
        private bgl.Material ImportMaterial(gltf.Model.Material material) {
            return new bgl.Material();  // TODO
        }

        private bgl.Mesh ImportMesh(gltf.Model.Mesh material) {
            return new bgl.Mesh();  // TODO
        }

       private void ImportBuffers() {
            // TODO
        }

        private void ImportBufferViews() {
            // TODO
        }
        
        private void ImportTexture() {
            // TODO
        }

    }
}