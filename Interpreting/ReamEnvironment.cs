using Ream.Lexing;

namespace Ream.Interpreting
{
    public class ReamEnvironment
    {
        private readonly Dictionary<string, object> Values = new();

        public void Define(string key, object value)
        {
            Values.Add(key, value);
        }

        public void Set(Token key, object value)
        {
            Values[key.Raw] = value;
        }

        public object Get(Token key)
        {
            string keyName = key.Raw;
            if (Values.ContainsKey(keyName))
            {
                return Values[keyName];
            }

            throw new RuntimeError(key, $"Undefined variable '{keyName}'"); // return null instead
        }
    }
}
