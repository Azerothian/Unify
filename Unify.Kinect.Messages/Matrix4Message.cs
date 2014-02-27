using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unify.Kinect.Messages
{
  [Serializable]
  public class Matrix4Message
  {
    public float M11 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 1, Column 2.
    public float M12 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 1, Column 3.
    public float M13 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 1, Column 4.
    public float M14 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 2, Column 1.
    public float M21 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 2, Column 2.
    public float M22 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 2, Column 3.
    public float M23 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 2, Column 4.
    public float M24 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 3, Column 1.
    public float M31 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 3, Column 2.
    public float M32 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 3, Column 3.
    public float M33 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 3, Column 4.
    public float M34 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 4, Column 1.
    public float M41 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 4, Column 2.
    public float M42 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 4, Column 3.
    public float M43 { get; set; }
    //
    // Summary:
    //     Gets or sets Row 4, Column 4.
    public float M44 { get; set; }
  }
}
