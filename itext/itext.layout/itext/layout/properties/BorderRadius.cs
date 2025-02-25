/*

This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
namespace iText.Layout.Properties {
    /// <summary>Represents a border radius.</summary>
    public class BorderRadius {
        /// <summary>The horizontal semi-major axis of the ellipse to use for the border in that corner.</summary>
        private UnitValue horizontalRadius;

        /// <summary>The vertical semi-major axis of the ellipse to use for the border in that corner.</summary>
        private UnitValue verticalRadius;

        /// <summary>
        /// Creates a
        /// <see cref="BorderRadius">border radius</see>
        /// with given value.
        /// </summary>
        /// <param name="radius">the radius</param>
        public BorderRadius(UnitValue radius) {
            this.horizontalRadius = radius;
            this.verticalRadius = radius;
        }

        /// <summary>
        /// Creates a
        /// <see cref="BorderRadius">border radius</see>
        /// with a given point value.
        /// </summary>
        /// <param name="radius">the radius</param>
        public BorderRadius(float radius) {
            this.horizontalRadius = UnitValue.CreatePointValue(radius);
            this.verticalRadius = this.horizontalRadius;
        }

        /// <summary>
        /// Creates a
        /// <see cref="BorderRadius">border radius</see>
        /// with given horizontal and vertical values.
        /// </summary>
        /// <param name="horizontalRadius">the horizontal radius of the corner</param>
        /// <param name="verticalRadius">the vertical radius of the corner</param>
        public BorderRadius(UnitValue horizontalRadius, UnitValue verticalRadius) {
            this.horizontalRadius = horizontalRadius;
            this.verticalRadius = verticalRadius;
        }

        /// <summary>
        /// Creates a
        /// <see cref="BorderRadius">border radius</see>
        /// with given horizontal and vertical point values.
        /// </summary>
        /// <param name="horizontalRadius">the horizontal radius of the corner</param>
        /// <param name="verticalRadius">the vertical radius of the corner</param>
        public BorderRadius(float horizontalRadius, float verticalRadius) {
            this.horizontalRadius = UnitValue.CreatePointValue(horizontalRadius);
            this.verticalRadius = UnitValue.CreatePointValue(verticalRadius);
        }

        /// <summary>
        /// Gets the horizontal radius of the
        /// <see cref="iText.Layout.Borders.Border">border's</see>
        /// corner.
        /// </summary>
        /// <returns>the horizontal radius</returns>
        public virtual UnitValue GetHorizontalRadius() {
            return horizontalRadius;
        }

        /// <summary>
        /// Gets the vertical radius of the
        /// <see cref="iText.Layout.Borders.Border">border's</see>
        /// corner.
        /// </summary>
        /// <returns>the vertical radius</returns>
        public virtual UnitValue GetVerticalRadius() {
            return verticalRadius;
        }
    }
}
