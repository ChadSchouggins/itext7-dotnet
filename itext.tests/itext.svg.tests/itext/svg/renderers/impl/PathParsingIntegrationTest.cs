/*
This file is part of the iText (R) project.
Copyright (c) 1998-2018 iText Group NV
Authors: iText Software.

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
using System;
using iText.Svg.Exceptions;
using iText.Svg.Renderers;
using iText.Test;

namespace iText.Svg.Renderers.Impl {
    public class PathParsingIntegrationTest : SvgIntegrationTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/svg/renderers/impl/PathParsingIntegrationTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/svg/renderers/impl/PathParsingIntegrationTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void BeforeClass() {
            ITextTest.CreateDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NormalTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "normal");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MixTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "mix");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void NoWhitespace() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "noWhitespace");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ZOperator() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "zOperator");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MissingOperandArgument() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "missingOperandArgument");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void DecimalPointHandlingTest() {
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "decimalPointHandling");
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidOperatorTest() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidOperator");
            }
            , NUnit.Framework.Throws.TypeOf<SvgProcessingException>());
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void InvalidOperatorCSensTest() {
            NUnit.Framework.Assert.That(() =>  {
                ConvertAndCompareVisually(sourceFolder, destinationFolder, "invalidOperatorCSens");
            }
            , NUnit.Framework.Throws.TypeOf<SvgProcessingException>());
;
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MoreThanOneHParam() {
            // TODO-2331 Update the cmp after the issue is resolved
            ConvertAndCompareVisually(sourceFolder, destinationFolder, "moreThanOneHParam");
        }

        [NUnit.Framework.Test]
        public virtual void DecimalPointParsingTest() {
            PathSvgNodeRenderer path = new PathSvgNodeRenderer();
            String input = "2.35.96";
            String expected = "2.35 .96";
            String actual = path.SeparateDecimalPoints(input);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }

        [NUnit.Framework.Test]
        public virtual void DecimalPointParsingSpaceTest() {
            PathSvgNodeRenderer path = new PathSvgNodeRenderer();
            String input = "2.35.96 3.25 .25";
            String expected = "2.35 .96 3.25 .25";
            String actual = path.SeparateDecimalPoints(input);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }

        [NUnit.Framework.Test]
        public virtual void DecimalPointParsingTabTest() {
            PathSvgNodeRenderer path = new PathSvgNodeRenderer();
            String input = "2.35.96 3.25\t.25";
            String expected = "2.35 .96 3.25\t.25";
            String actual = path.SeparateDecimalPoints(input);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }

        [NUnit.Framework.Test]
        public virtual void DecimalPointParsingMinusTest() {
            PathSvgNodeRenderer path = new PathSvgNodeRenderer();
            String input = "2.35.96 3.25-.25";
            String expected = "2.35 .96 3.25-.25";
            String actual = path.SeparateDecimalPoints(input);
            NUnit.Framework.Assert.AreEqual(expected, actual);
        }
    }
}