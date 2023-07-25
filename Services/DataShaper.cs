using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        //Seçilen elemanların datalarını tutmak için tanımladık
        public PropertyInfo[] Properties { get; set; }

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        //ShapedData listesi döner
        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredFields = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredFields);
        }

        //ShapedData objesi döner
        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
        }

        //Seçilen dataların listlerini tutmak için tanımladık
        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            //Seçilen alanları içeren liste oluşturduk
            var requiredFields = new List<PropertyInfo>();

            if (!string.IsNullOrWhiteSpace(fieldsString))
            {
                //Listenin içeriğini split eder ve boş olan alanları otomatik olarak siler (yani array elde etmiş oluruz)
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var field in fields)
                {
                    var propety = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(),
                        StringComparison.InvariantCultureIgnoreCase));

                    if (propety is null)
                        continue;

                    requiredFields.Add(propety);
                }
            }

            else
            {
                requiredFields = Properties.ToList();
            }

            return requiredFields;

        }

        private ShapedEntity FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ShapedEntity();

            foreach (var property in requiredProperties)
            {
                var objectPropetyValue = property.GetValue(entity);

                shapedObject.Entity.TryAdd(property.Name, objectPropetyValue);
            }

            //varlığın Id lerini alır
            var objectProperty = entity.GetType().GetProperty("Id");

            //varlığın alınan Id lerini shapedObject in Id alanına atar
            shapedObject.Id = (int)objectProperty.GetValue(entity);

            return shapedObject;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, 
            IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ShapedEntity>();

            foreach (var entity in entities)
            {
                var shapedObject = FetchDataForEntity(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }

            return shapedData;
        }
    }
}
