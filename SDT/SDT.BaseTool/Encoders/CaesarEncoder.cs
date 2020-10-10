﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    /// <summary>
    /// Encodes using caesar cypher.
    /// </summary>
    public class CaesarEncoder : IEncoder<int>
    {
        /// <summary>
        /// Encodes text using specified key,
        /// time complexity: O(n),
        /// space complexity: O(n),
        /// where n - text length.
        /// </summary>
        /// <param name="text">Text to be encoded.</param>
        /// <param name="key">Key that will be used to encode the text.</param>
        /// <returns>Encoded text.</returns>
        public string Encode(string text, int key) => Cipher(text, key);

        /// <summary>
        /// Decodes text that was encoded using specified key,
        /// time complexity: O(n),
        /// space complexity: O(n),
        /// where n - text length.
        /// </summary>
        /// <param name="text">Text to be decoded.</param>
        /// <param name="key">Key that was used to encode the text.</param>
        /// <returns>Decoded text.</returns>
        public string Decode(string text, int key) => Cipher(text, -key);

        private static string Cipher(string text, int key)
        {
            var newText = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    var num = Convert.ToInt32(text[i].ToString());
                    var sum = num + key;
                    var dig = sum % 10;
                    if (dig < 0)
                    {
                        dig = 10 + dig;
                    }
                    newText.Append(dig.ToString());
                    continue;
                }
                else if (!char.IsLetter(text[i]))
                {
                    newText.Append(text[i]);
                    continue;
                }

                var letterA = char.IsUpper(text[i]) ? 'A' : 'a';
                var letterZ = char.IsUpper(text[i]) ? 'Z' : 'z';

                var c = text[i] + key;
                c -= c > letterZ ? (26 * (1 + (c - letterZ - 1) / 26)) : 0;
                c += c < letterA ? (26 * (1 + (letterA - c - 1) / 26)) : 0;

                newText.Append((char)c);
            }

            return newText.ToString();
        }
    }
}