using System;
using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XmlParser
{
    [ComVisible(true)]
    public class RibbonController : ExcelRibbon
    {        
        public override string GetCustomUI(string RibbonID)
        {
            return @"
      <customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui' loadImage='LoadImage'>
      <ribbon>
        <tabs>
          <tab id='tab1' label='Xml Parser'>
            <group id='group1' label='Load XML'>
              <button id='button1' image='icon' size='large' label='Load Data' onAction='OnButtonLoadPressed'/>
              <button id='button2' image='icon' size='large' label='Save Data' onAction='OnButtonSavePressed'/>
            </group >
          </tab>
        </tabs>
      </ribbon>
    </customUI>";
        }

        public void OnButtonLoadPressed(IRibbonControl control)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.CheckFileExists = true;
                ofd.Filter = "XML files|*.xml";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var document = new XmlDocumentWrapper();
                    var excelData = new ExcelData();
                    excelData.Write(document.Load(ofd.FileName));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public void OnButtonSavePressed(IRibbonControl control)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.CheckPathExists = true;
                sfd.Filter = "XML files|*.xml";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var document = new XmlDocumentWrapper();
                    var excelData = new ExcelData();
                    document.LoadFrom(excelData.Read());
                    document.Save(sfd.FileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }

}
