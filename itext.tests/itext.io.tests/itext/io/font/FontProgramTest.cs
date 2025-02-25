/*
This file is part of the iText (R) project.
    Copyright (c) 1998-2021 iText Group NV
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
using System.Collections.Generic;
using System.Reflection;
using iText.IO.Font.Constants;
using iText.IO.Util;
using iText.Test;

namespace iText.IO.Font {
    public class FontProgramTest : ExtendedITextTest {
        private const String notExistingFont = "some-font.ttf";

        [NUnit.Framework.Test]
        public virtual void ExceptionMessageTest() {
            NUnit.Framework.Assert.That(() =>  {
                FontProgramFactory.CreateFont(notExistingFont);
            }
            , NUnit.Framework.Throws.InstanceOf<System.IO.IOException>().With.Message.EqualTo(MessageFormatUtil.Format(iText.IO.IOException._1NotFoundAsFileOrResource, notExistingFont)))
;
        }

        [NUnit.Framework.Test]
        public virtual void BoldTest() {
            FontProgram fp = FontProgramFactory.CreateFont(StandardFonts.HELVETICA);
            fp.SetBold(true);
            NUnit.Framework.Assert.IsTrue((fp.GetPdfFontFlags() & (1 << 18)) != 0, "Bold expected");
            fp.SetBold(false);
            NUnit.Framework.Assert.IsTrue((fp.GetPdfFontFlags() & (1 << 18)) == 0, "Not Bold expected");
        }

        [NUnit.Framework.Test]
        public virtual void FontCacheTest()
        {
            FontProgramFactory.ClearRegisteredFonts();
            FontProgramFactory.ClearRegisteredFontFamilies();
            int cacheSize = -1;
            try
            {
                FieldInfo f = typeof(FontCache).GetField("fontCache", BindingFlags.NonPublic | BindingFlags.Static);
                IDictionary<FontCacheKey, FontProgram> cachedFonts = (IDictionary<FontCacheKey, FontProgram>)f.GetValue(null);
                cachedFonts.Clear();
                FontProgramFactory.RegisterFontDirectory(iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
                                                             .CurrentContext.TestDirectory) + "/resources/itext/io/font/otf/");
                cacheSize = cachedFonts.Count;
            }
            catch (Exception) { }

            foreach (String s in FontProgramFactory.GetRegisteredFonts())
                Console.WriteLine(s);

            NUnit.Framework.Assert.AreEqual(43, FontProgramFactory.GetRegisteredFonts().Count);
            NUnit.Framework.Assert.IsTrue(FontProgramFactory.GetRegisteredFonts().Contains("free sans lihavoitu"));
            NUnit.Framework.Assert.AreEqual(0, cacheSize);
        }

        [NUnit.Framework.Test]
        public void RegisterDirectoryType1Test()
        {
            FontProgramFactory.RegisterFontDirectory(iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
                                                         .CurrentContext.TestDirectory) + "/resources/itext/io/font/type1/");
            FontProgram computerModern = FontProgramFactory.CreateRegisteredFont("computer modern");
            FontProgram cmr10 = FontProgramFactory.CreateRegisteredFont("cmr10");
            NUnit.Framework.Assert.NotNull(computerModern);
            NUnit.Framework.Assert.NotNull(cmr10);
        }
    }
}
