/*
 * CollectionArea.cs
 * 
 * This file defines the `Node` class, which is a core component of the graph used in the
 * VRPT-SWTS (Vehicle Routing Problem with Trash collection and Soft Time and Service Windows).
 * 
 * This file defines the `CollectionArea` class, which extends the `Node` class and
 * represents a specific location in the VRPT-SWTS (Vehicle Routing Problem with Trash collection
 * and Soft Time and Service Windows) scenario where waste collection tasks are performed.
 * 
 * In addition to the basic node properties (ID and coordinates), a `CollectionArea` has:
 *  - A demand value representing the amount of waste to be collected.
 *  - A processing time indicating how long it takes to complete the collection at this location.
 *
 * Author: Adrián García Rodríguez
 * Date: 22/04/2025
 */

namespace VrptSwts.Graph;

/// <summary>
/// Represents a node in the graph where a collection task must be performed.
/// Inherits from Node and adds collection-specific attributes.
/// </summary>
internal class CollectionArea : Node
{

  /// <summary>
  /// Gets the demand at this collection area (e.g., volume of waste).
  /// </summary>
  public int Demand { get; }

  /// <summary>
  /// Gets the time required to process the collection at this area.
  /// </summary>
  public double ProcessingTime { get; }

  /// <summary>
  /// Initializes a new instance of the CollectionArea class.
  /// </summary>
  /// <param name="id">Unique identifier of the area.</param>
  /// <param name="coordinates">Tuple containing (x, y) position of the area.</param>
  /// <param name="demand">Amount of waste to be collected from this area.</param>
  /// <param name="processingTime">Time required to process the collection at this area.</param>
  public CollectionArea(string id, Tuple<int, int> coordinates, int demand,
                        double processingTime)
      : base(id, coordinates)
  {
    Demand = demand;
    ProcessingTime = processingTime;
  }
}