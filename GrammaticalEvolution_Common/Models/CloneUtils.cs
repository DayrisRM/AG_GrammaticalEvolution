using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GrammaticalEvolution_Common.Models
{
    public static class CloneUtils
    {
        public static T Clone<T>(T origen)
        {
            // Verificamos que sea serializable antes de hacer la copia            
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("La clase " + typeof(T).ToString() + " no es serializable");

            // En caso de ser nulo el objeto, se devuelve tal cual
            if (ReferenceEquals(origen, null))
                return default;

            //Creamos un stream en memoria            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                try
                {
                    formatter.Serialize(stream, origen);
                    stream.Seek(0, SeekOrigin.Begin);
                    //Deserializamos la porcón de memoria en el nuevo objeto                
                    return (T)formatter.Deserialize(stream);
                }
                catch (SerializationException ex)
                { throw new ArgumentException(ex.Message, ex); }
                catch { throw; }
            }
        }
    }
}
