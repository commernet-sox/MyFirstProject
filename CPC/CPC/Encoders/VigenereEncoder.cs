﻿using System;
using System.Text;

namespace CPC
{
    /// <summary>
    /// Encodes using vigenere cypher.
    /// </summary>
    public class VigenereEncoder : IEncoder<string>
    {
        private readonly CaesarEncoder caesarEncoder = new CaesarEncoder();

        /// <summary>
        /// Encodes text using specified key,
        /// time complexity: O(n),
        /// space complexity: O(n),
        /// where n - text length.
        /// </summary>
        /// <param name="text">Text to be encoded.</param>
        /// <param name="key">Key that will be used to encode the text.</param>
        /// <returns>Encoded text.</returns>
        public string Encode(string text, string key) => Cipher(text, key, caesarEncoder.Encode);

        /// <summary>
        /// Decodes text that was encoded using specified key,
        /// time complexity: O(n),
        /// space complexity: O(n),
        /// where n - text length.
        /// </summary>
        /// <param name="text">Text to be decoded.</param>
        /// <param name="key">Key that was used to encode the text.</param>
        /// <returns>Decoded text.</returns>
        public string Decode(string text, string key) => Cipher(text, key, caesarEncoder.Decode);

        private string Cipher(string text, string key, Func<string, int, string> symbolCipher)
        {
            key = AppendKey(key, text.Length);
            var encodedTextBuilder = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    var digit = symbolCipher(text[i].ToString(), i);
                    encodedTextBuilder.Append(digit);
                    continue;
                }
                else if (!char.IsLetter(text[i]))
                {
                    encodedTextBuilder.Append(text[i]);
                    continue;
                }

                var letterZ = char.IsUpper(key[i]) ? 'Z' : 'z';

                var encodedSymbol = symbolCipher(text[i].ToString(), letterZ - key[i]);
                encodedTextBuilder.Append(encodedSymbol);
            }

            return encodedTextBuilder.ToString();
        }

        private string AppendKey(string key, int length)
        {
            var keyBuilder = new StringBuilder(key, length);
            while (keyBuilder.Length < length)
            {
                keyBuilder.Append(key);
            }

            return keyBuilder.ToString();
        }
    }
}
