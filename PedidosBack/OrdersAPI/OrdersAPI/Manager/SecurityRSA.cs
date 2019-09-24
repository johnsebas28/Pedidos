using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace OrdersAPI.Manager
{
    public class SecurityRSA
    {
        RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private string privKeyString;
        private string pubKeyString;
        RSAParameters pubKey;

        RSAParameters privKey;
        public SecurityRSA()
        {
            //how to get the private key
            privKey = csp.ExportParameters(true);
        }

        public string GeneratePublicKey()
        {
            pubKey = csp.ExportParameters(false);
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, pubKey);
            //get the string from the stream
            pubKeyString = sw.ToString();
            return pubKeyString;
        }

        public string GeneratePrivateKey()
        {
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, privKey);
            //get the string from the stream
            privKeyString = sw.ToString();
            return privKeyString;
        }

        public string Encrypt(string pubKeyString, string plainTextData)
        {
            var sr = new System.IO.StringReader(pubKeyString);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            pubKey = (RSAParameters)xs.Deserialize(sr);

            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);
            //apply pkcs#1.5 padding and encrypt our data 
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            //we might want a string representation of our cypher text... base64 will do
            var cypherText = Convert.ToBase64String(bytesCypherText);

            return cypherText;
        }

        public string Decrypt(string privKeyString, string cypherText)
        {

            var sr = new System.IO.StringReader(privKeyString);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            privKey = (RSAParameters)xs.Deserialize(sr);
            //first, get our bytes back from the base64 string ..
            byte[] bytesCypherText = Convert.FromBase64String(cypherText);
            //we want to decrypt, therefore we need a csp and load our private key
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            //decrypt and strip pkcs#1.5 padding
            byte[] bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            return System.Text.Encoding.Unicode.GetString(bytesPlainTextData);

        }


    }
}
