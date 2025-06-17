/*
 * Universidad de La Laguna
 * Escuela Superior de Ingeniería y Tecnología
 * Grado en Ingeniería Informática
 * Diseño y Análisis de Algoritmos 2024-2025
 *
 * File: ExceptionStringToInt.cs
 * Authors: Roberto Padrón Castañeda & Adrián García Rodríguez
 * Date: 13/02/2025
 * Description: 
 *     This file contains the implementation of the ExceptionStringToInt class, 
 *     which provides a safe method to convert a string to an integer. 
 *     It handles exceptions related to invalid formats and number overflows.
 */

using System.Globalization;

namespace VrptSwts.Utils;

/// <summary>
/// A utility class that provides a method to safely convert a string to an integer. 
/// It ensures proper handling of invalid input formats and numeric overflow conditions.
/// </summary>
public static class ExceptionStringToNumber
{
  /// <summary>
  /// Attempts to convert a given string into an integer while handling potential exceptions.
  /// </summary>
  /// <param name="stringToConvert">The input string that needs to be converted to an integer.</param>
  /// <returns>The integer value obtained from the input string.</returns>
  /// <exception cref="ArgumentException">
  /// Thrown when the input string is either not in a valid numeric format or exceeds 
  /// the allowable range of an integer.
  /// </exception>
  public static int ConvertToInt(string stringToConvert)
  {
    try
    {
      return int.Parse(stringToConvert);
    }
    catch (FormatException)
    {
      throw new ArgumentException($"Invalid number format for input '{stringToConvert}'");
    }
    catch (OverflowException)
    {
      throw new ArgumentException($"Input '{stringToConvert}' is too large or too small.");
    }
  }

  /// <summary>
  /// Attempts to convert a given string into a double while handling potential exceptions.
  /// </summary>
  /// <param name="stringToConvert">The input string that needs to be converted to an double.</param>
  /// <returns>The integer value obtained from the input string.</returns>
  /// <exception cref="ArgumentException">
  /// Thrown when the input string is either not in a valid numeric format or exceeds 
  /// the allowable range of an integer.
  /// </exception>
  public static double ConvertToDouble(string stringToConvert)
  {
    try
    {
      return double.Parse(stringToConvert, CultureInfo.InvariantCulture);
    }
    catch (FormatException)
    {
      throw new ArgumentException($"Invalid number format for input '{stringToConvert}'");
    }
    catch (OverflowException)
    {
      throw new ArgumentException($"Input '{stringToConvert}' is too large or too small.");
    }
  }
}
