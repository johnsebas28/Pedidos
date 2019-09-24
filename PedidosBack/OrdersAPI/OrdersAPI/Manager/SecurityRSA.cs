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
        private const string privKeyString = @"<?xml version=""1.0"" encoding=""utf - 16""?><RSAParameters xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  <D>OcS5jima8gWRDRqoXIU5R8lzxhdbGQk8GpqcnAjARGZXTPYDvRMN2yYx03EkiGzeVk76HuLE0vjMQqdkuwM66Dr2y7vIKfoDUyvioCCmA8Qf86zA0zh4Ahfb/bvHig/6QSJmdhSnATIsOchPlGjuo5UdhwCYAVk2IiPU0i0MGmA2iNHAtH47KHcpU9oxFpuL8B+rug/XQ8QCCcMwM12wXdKAQYXIQGeIu4TMtn31GqzvMFDsogwGHoWFxeE4HLWNj/2K6uR2kXPYIZGxNSIV3kt3f4A14gvEMBruEQ2sjbj+WA8S6qDgUUqwSPRDdRIAgoPpn70RO1upFxODq1+JBQ==</D>  <DP>rWAKXH6GHHoq/+b0YCYbQ0yna0F1Ffz4UUOxW+TUf1+6u2U50DY7jF/u1iPCqEkPZqgCy2pQeAdNhMkOYpJrFCFADoAolO0+1ncL0yhUnTpCPSU/Kb3gFvaf3ZvihOh2CKN/PIckzr4sSpTAHEK61zRL+wAGiagTbylD4rmRhH0=</DP>  <DQ>IydEOr5l/lbopxQlx6Fl/yDQjiLKzw5IpMDCiQo4rlwOw672diyCBIBzQadQTkhnfq/QLYNTSO9SnrNmsQaZ69L5KDSqx9LtPmj2rvnbkefUB5OjJ5deqDlJUYwLMeRU0V04RKND7z/kyjhR0tmPAF8n8hVa/fRCoRPL025lD9M=</DQ>  <Exponent>AQAB</Exponent>  <InverseQ>NTgZQBJFhvDBjNYKO/H5fNU3IGQ57gV+EJBPx7IigRRgScmU84OIuknK/scAXnQKCIpniTFGhHme+1gcJ1HxJhVbaVoRau+FUPQiOPK9g46jWRK2GHictVG5wg27yipnjlOGqnR/GiP4oFCfzO/P+St8cwu3ZFucYaPhlE+arUY=</InverseQ>  <Modulus>xopm5oxprRFRsx/8A+M9ulx963K7Y/P8j6En3ga+TOf6kSQiyQI6RFVlSUXZfdLnCDGEU+QSfv6pHrK7M+MHJfNXNExAkhesFU7lRWkbDoqDv+RZ8lnNbCLTTY2An0RS36QrBRpeIjIYFW7y0B7B1gfGRj2pKrG2V5VyIBxUkcCRGEsUpP+c80gxk6rp/of5i3Llu5PGnI44uNe3K1Y4xzlgybEUgARHiEJpWO2oW9Jf/D522+QnAT8LPjEKwWR2rRIpzgMFX0nXZbo/M05ibZHcWGyNPozSecqhgn6OZIsyJnE2rqrIOxpqya/otDepj20a/leIzwMgwPxH+yGfaQ==</Modulus>  <P>/M2XYk1FigJMT8v8/NAp+V6k7MPMU0KqHB04/TLjSIfX9yj1ihlJo0VKKKL+TY0mTs18uMC4SAnIXVsvDlLZFwYkHAYxFJ4W06nwc6ttuEu6MJfN3zLT4r1L4TgJBst9pH1ldrlNB0AjxgAswFlwxmN3KXQZWBreA9Td1kUf608=</P>  <Q>yQ0lECqNhXXHbvu1HBBM3PdB/idLtWg2OKRpCme+PhQWPtDAx4XByCi+Y2IkF+AjMDe8j67ghyx4U5lDxLFFVQaRVD3+oDg7i4Vrbyf6uOvep5dUI7Y8tmMAwX/ToVrf7z7V7futwPODNVoCy92jD9fLTddR0WGHhm6x8oLhu8c=</Q></RSAParameters>";
        private string pubKeyString;
        RSAParameters pubKey;
        RSAParameters privKey;

        public string GeneratePublicKey()
        {
            var sr = new System.IO.StringReader(privKeyString);
            //we need a deserializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //get the object back from the stream
            privKey = (RSAParameters)xs.Deserialize(sr);
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);
            pubKey = csp.ExportParameters(false);
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs2= new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs2.Serialize(sw, pubKey);
            //get the string from the stream
            pubKeyString = sw.ToString();
            return pubKeyString;
        }

        public string GeneratePrivateKey()
        {
            privKey = csp.ExportParameters(true);
            //we need some buffer
            var sw = new System.IO.StringWriter();
            //we need a serializer
            var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            //serialize the key into the stream
            xs.Serialize(sw, privKey);
            //get the string from the stream
            return sw.ToString();
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

        public string Decrypt( string cypherText)
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
