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
using System.IO;
using iText.IO;
using iText.IO.Util;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Kernel.Utils {
    public class CompareToolTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/kernel/utils/CompareToolTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/utils/CompareToolTest/";

        [NUnit.Framework.OneTimeSetUp]
        public static void SetUp() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        [NUnit.Framework.Test]
        public virtual void CompareToolErrorReportTest01() {
            CompareTool compareTool = new CompareTool();
            compareTool.SetCompareByContentErrorsLimit(10);
            compareTool.SetGenerateCompareByContentXmlReport(true);
            String outPdf = sourceFolder + "simple_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_simple_pdf.pdf";
            String result = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder);
            System.Console.Out.WriteLine("\nRESULT:\n" + result);
            NUnit.Framework.Assert.IsNotNull(result, "CompareTool must return differences found between the files");
            NUnit.Framework.Assert.IsTrue(result.Contains("differs on page [1, 2]."));
            // Comparing the report to the reference one.
            NUnit.Framework.Assert.IsTrue(compareTool.CompareXmls(destinationFolder + "simple_pdf.report.xml", sourceFolder
                 + "cmp_report01.xml"), "CompareTool report differs from the reference one");
        }

        [NUnit.Framework.Test]
        public virtual void CompareToolErrorReportTest02() {
            CompareTool compareTool = new CompareTool();
            compareTool.SetCompareByContentErrorsLimit(10);
            compareTool.SetGenerateCompareByContentXmlReport(true);
            String outPdf = sourceFolder + "tagged_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_tagged_pdf.pdf";
            String result = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder);
            System.Console.Out.WriteLine("\nRESULT:\n" + result);
            NUnit.Framework.Assert.IsNotNull(result, "CompareTool must return differences found between the files");
            NUnit.Framework.Assert.IsTrue(result.Contains("Compare by content fails. No visual differences"));
            // Comparing the report to the reference one.
            NUnit.Framework.Assert.IsTrue(compareTool.CompareXmls(destinationFolder + "tagged_pdf.report.xml", sourceFolder
                 + "cmp_report02.xml"), "CompareTool report differs from the reference one");
        }

        [NUnit.Framework.Test]
        public virtual void CompareToolErrorReportTest03() {
            CompareTool compareTool = new CompareTool();
            compareTool.SetCompareByContentErrorsLimit(10);
            compareTool.SetGenerateCompareByContentXmlReport(true);
            String outPdf = sourceFolder + "screenAnnotation.pdf";
            String cmpPdf = sourceFolder + "cmp_screenAnnotation.pdf";
            String result = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder);
            System.Console.Out.WriteLine("\nRESULT:\n" + result);
            NUnit.Framework.Assert.IsNotNull(result, "CompareTool must return differences found between the files");
            NUnit.Framework.Assert.IsTrue(result.Contains("Compare by content fails. No visual differences"));
            // Comparing the report to the reference one.
            NUnit.Framework.Assert.IsTrue(compareTool.CompareXmls(destinationFolder + "screenAnnotation.report.xml", sourceFolder
                 + "cmp_report03.xml"), "CompareTool report differs from the reference one");
        }

        [NUnit.Framework.Test]
        public virtual void CompareToolErrorReportTest04() {
            // Test space in name
            CompareTool compareTool = new CompareTool();
            compareTool.SetCompareByContentErrorsLimit(10);
            compareTool.SetGenerateCompareByContentXmlReport(true);
            String outPdf = sourceFolder + "simple_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_simple_pdf_with_space .pdf";
            String result = compareTool.CompareByContent(outPdf, cmpPdf, destinationFolder);
            System.Console.Out.WriteLine("\nRESULT:\n" + result);
            NUnit.Framework.Assert.IsNotNull(result, "CompareTool must return differences found between the files");
            NUnit.Framework.Assert.IsTrue(result.Contains("differs on page [1, 2]."));
            // Comparing the report to the reference one.
            NUnit.Framework.Assert.IsTrue(compareTool.CompareXmls(destinationFolder + "simple_pdf.report.xml", sourceFolder
                 + "cmp_report01.xml"), "CompareTool report differs from the reference one");
        }

        [NUnit.Framework.Test]
        public virtual void DifferentProducerTest() {
            String expectedMessage = "Document info fail. Expected: \"iText\u00ae <version> \u00a9<copyright years> iText Group NV (iText Software; licensed version)\", actual: \"iText\u00ae <version> \u00a9<copyright years> iText Group NV (AGPL-version)\"";
            String licensed = sourceFolder + "producerLicensed.pdf";
            String agpl = sourceFolder + "producerAGPL.pdf";
            NUnit.Framework.Assert.AreEqual(expectedMessage, new CompareTool().CompareDocumentInfo(agpl, licensed));
        }

        [NUnit.Framework.Test]
        public virtual void VersionReplaceTest() {
            String initial = "iText® 1.10.10-SNAPSHOT (licensed to iText) ©2000-2018 iText Group NV";
            String replacedExpected = "iText® <version> (licensed to iText) ©<copyright years> iText Group NV";
            NUnit.Framework.Assert.AreEqual(replacedExpected, new CompareTool().ConvertProducerLine(initial));
        }

        [NUnit.Framework.Test]
        public virtual void GsEnvironmentVariableIsNotSpecifiedExceptionTest() {
            String outPdf = sourceFolder + "simple_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_simple_pdf.pdf";
            new CompareTool(null, null).CompareVisually(outPdf, cmpPdf, destinationFolder, "diff_");
            NUnit.Framework.Assert.IsTrue(new FileInfo(destinationFolder + "diff_1.png").Exists);
        }

        [NUnit.Framework.Test]
        public virtual void GsEnvironmentVariableSpecifiedIncorrectlyTest() {
            NUnit.Framework.Assert.That(() =>  {
                String outPdf = sourceFolder + "simple_pdf.pdf";
                String cmpPdf = sourceFolder + "cmp_simple_pdf.pdf";
                new CompareTool("unspecified", null).CompareVisually(outPdf, cmpPdf, destinationFolder, "diff_");
            }
            , NUnit.Framework.Throws.InstanceOf<CompareTool.CompareToolExecutionException>().With.Message.EqualTo(IoExceptionMessage.GS_ENVIRONMENT_VARIABLE_IS_NOT_SPECIFIED))
;
        }

        [NUnit.Framework.Test]
        public virtual void CompareCommandIsNotSpecifiedTest() {
            String outPdf = sourceFolder + "simple_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_simple_pdf.pdf";
            String gsExec = SystemUtil.GetEnvironmentVariable(GhostscriptHelper.GHOSTSCRIPT_ENVIRONMENT_VARIABLE);
            if (gsExec == null) {
                gsExec = SystemUtil.GetEnvironmentVariable("gsExec");
            }
            String result = new CompareTool(gsExec, null).CompareVisually(outPdf, cmpPdf, destinationFolder, "diff_");
            NUnit.Framework.Assert.IsFalse(result.Contains(IoExceptionMessage.COMPARE_COMMAND_IS_NOT_SPECIFIED));
            NUnit.Framework.Assert.IsTrue(new FileInfo(destinationFolder + "diff_1.png").Exists);
        }

        [NUnit.Framework.Test]
        [LogMessage(IoExceptionMessage.COMPARE_COMMAND_SPECIFIED_INCORRECTLY)]
        public virtual void CompareCommandSpecifiedIncorrectlyTest() {
            String outPdf = sourceFolder + "simple_pdf.pdf";
            String cmpPdf = sourceFolder + "cmp_simple_pdf.pdf";
            String gsExec = SystemUtil.GetEnvironmentVariable(GhostscriptHelper.GHOSTSCRIPT_ENVIRONMENT_VARIABLE);
            if (gsExec == null) {
                gsExec = SystemUtil.GetEnvironmentVariable("gsExec");
            }
            String result = new CompareTool(gsExec, "unspecified").CompareVisually(outPdf, cmpPdf, destinationFolder, 
                "diff_");
            NUnit.Framework.Assert.IsTrue(result.Contains(IoExceptionMessage.COMPARE_COMMAND_SPECIFIED_INCORRECTLY));
        }

        [NUnit.Framework.Test]
        public virtual void CompareVisuallyDiffTestTest() {
            String outPdf = sourceFolder + "compareVisuallyDiffTestTest1.pdf";
            String cmpPdf = sourceFolder + "compareVisuallyDiffTestTest2.pdf";
            String result = new CompareTool().CompareVisually(outPdf, cmpPdf, destinationFolder, "diff_");
            System.Console.Out.WriteLine("\nRESULT:\n" + result);
            NUnit.Framework.Assert.IsTrue(result.Contains("differs on page [1, 2]."));
            NUnit.Framework.Assert.IsTrue(new FileInfo(destinationFolder + "diff_1.png").Exists);
            NUnit.Framework.Assert.IsTrue(new FileInfo(destinationFolder + "diff_2.png").Exists);
        }
    }
}
