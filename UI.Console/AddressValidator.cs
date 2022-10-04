using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace UI.Console
{
    // source: https://github.com/Sofoca/CoinUtils/blob/master/CoinUtils/AddressValidator.cs

    public static class AddressValidator
    {
        // Base58 prefixes
        private static readonly int[] BitcoinBase58MainnetPrefixes = { 0, 5 };

        public static bool IsValidAddress(string address)
        {
            if (address == null) throw new ArgumentNullException(new(address));

            return ValidateBase58Address(address, BitcoinBase58MainnetPrefixes);
        }

        private static bool ValidateBase58Address(string address, int[] prefixes)
        {
            return Base58.Decode(address, prefixes) != null;
        }

        private static class Base58 
        {

            internal static byte[] Decode(string address, int[] prefixes)
            {
                if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
                if (prefixes == null) throw new ArgumentNullException(nameof(prefixes));
                if (prefixes.Length == 0)
                    throw new ArgumentException("Error at Base58 decoding, at least one prefix is required", nameof(prefixes));
                var decoded = ToByteArrayCheckAndStripChecksum(address);
                if (decoded == null || decoded.Length != 21) throw new FormatException("Error at Base58 decoding, " +
                                                                                       "address too short or could not be decoded");
                if (!prefixes.Contains(decoded[0])) throw new FormatException("Error at Base58 decoding, wrong prefix");
                return decoded;
            }

            private static byte[] ToByteArrayCheckAndStripChecksum(string inputBase58)
            {
                // convert base58 encoding to byte array
                var inputArray = ToByteArray(inputBase58);
                if (inputArray == null || inputArray.Length < 4) throw new FormatException("Error at Base58 decoding, encoding too short");

                // compute and check checksum
                var hasher = new SHA256Managed();
                var hash = hasher.ComputeHash(inputArray.SubArray(0, inputArray.Length - 4));
                hash = hasher.ComputeHash(hash);
                if (!inputArray.SubArray(21, 4).SequenceEqual(hash.SubArray(0, 4)))
                    throw new FormatException("Error at Base58 decoding, bad checksum");

                // strip checksum and return
                return inputArray.SubArray(0, inputArray.Length - 4);
            }

            private static byte[] ToByteArray(string inputBase58)
            {
                var outputValue = new BigInteger(0);
                const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
                foreach (var character in inputBase58)
                {
                    if (alphabet.Contains(character))
                        outputValue = BigInteger.Add(new BigInteger(alphabet.IndexOf(character)),
                            BigInteger.Multiply(outputValue, new BigInteger(58)));
                    else throw new FormatException("Error at Base58 decoding, invalid character in base58 address: " + character);
                }

                var outputArray = outputValue.ToByteArray(true, true);
                // interpret leading 1s as leading zero bytes as per specification
                foreach (var character in inputBase58)
                {
                    if (character != '1') break;
                    var extendedArray = new byte[outputArray.Length + 1];
                    Array.Copy(outputArray, 0, extendedArray, 1, outputArray.Length);
                    outputArray = extendedArray;
                }
                return outputArray;
            }
        }
    }

    internal static class ArrayExtensions
    {
        /*
         * <summary>
         * Generates sub array with subset of consecutive elements
         * </summary>
         *
         * <param name="index">index of the original array to start the sub array at</param>
         * <param name="length">length of the desired sub array</param>
         * 
         * <returns>sub array from original array starting at index and with length</returns>
         */
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
