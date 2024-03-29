using System;
using System.IO;



namespace bgl {

    public class GLTF {


        //

        public static void MainProgram()  {
            Console.WriteLine("Hello, World!");
            const string path = "C:/Users/Bastian/Code/wpf-demo/GLTF/";
            const string modelPath = "C:/Users/Bastian/Code/wpf-demo/GLTF/DamagedHelmet.gltf";  // TODO
            try {
                FileStream stream = System.IO.File.Open(modelPath, FileMode.Open);
                System.Text.Json.JsonDocument document = System.Text.Json.JsonDocument.Parse(stream);

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("Model: " + Path.GetFileName(path));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();

                var root = document.RootElement;
                System.Text.Json.JsonElement element = root;

                var images = root.GetProperty("images");
                var meshes = root.GetProperty("meshes");


                var materials = root.GetProperty("materials");
                var textures = root.GetProperty("textures");
                var buffers = root.GetProperty("buffers");
                
                string PrintSize(ulong size) {
                    string value = size.ToString();
                    string unit = "Bytes";

                    ulong _number = size;
                    if (size > 1024 * 1024) {
                        unit = "MB";
                        _number /= 1024 * 1024;
                    }
                    else if (size > 1024) {
                        unit = "KB";
                        _number /= 1024;
                    }
                    
                    return _number + " " + unit;

                }
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write( "* " + images.GetArrayLength() + " images");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
                for (int i = 0; i < images.GetArrayLength(); ++i) {
                    var image = images[i];
                    string name =  image.GetProperty("uri").ToString();

                    long length = new System.IO.FileInfo(path + name).Length;

                    var line = String.Format(@"{0} ({1})",
                                            name,
                                            PrintSize((ulong) length));
                    Console.WriteLine("\t" + line);
                }


                Console.WriteLine( "* " + textures.GetArrayLength() + "textures");




                Console.WriteLine( "--> #buffers:" + buffers.GetArrayLength());
                for (int i = 0; i < buffers.GetArrayLength(); ++i) {
                    var buffer = buffers[i];
                    string name =  buffer.GetProperty("uri").ToString();
                    
                    ulong size =  buffer.GetProperty("byteLength").GetUInt64();


                    Console.Write("\t" + (name != " "   ? name : "unnamed material")    );
                    Console.WriteLine(" " + PrintSize(size));
                }
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.Write("* " + materials.GetArrayLength() + " materials");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();

                for (int i = 0; i < materials.GetArrayLength(); ++i) {
                    var material = materials[i];
                    string name =  material.GetProperty("name").ToString();
                    Console.WriteLine("\t" + (name != " "   ? name : "unnamed material")    );


                    // TODO: aonymous functions

                    string normalTextureTexture =  material.GetProperty("normalTexture").ToString();
                    string occlusionTextureTexture =  material.GetProperty("occlusionTexture").ToString();
                    string metallicRoughnessTexture =  material.GetProperty("pbrMetallicRoughness").ToString();
                    string emissiveTextureTexture =  material.GetProperty("emissiveTexture").ToString();


                    if (normalTextureTexture.Length >  0) {
                        Console.WriteLine("\t\t * Normal Texture");
                    }
                    if (occlusionTextureTexture.Length > 0 ) {
                        Console.WriteLine("\t\t * Occlusion Texture ");
                    }
                    if (metallicRoughnessTexture.Length> 0 ) {
                        Console.WriteLine("\t\t * MetallicRoughnessTexture ");
                    }
                    if (emissiveTextureTexture.Length > 0 ) {
                        Console.WriteLine("\t\t * emissiveTextureTexture ");
                    }

                    // TODO: number indices
                    // TODO: number vertices
                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }

        }
    }


}

//Console.Write( "root: " + document.RootElement.ToString());
