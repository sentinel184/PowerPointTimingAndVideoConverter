﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProj
{
    internal class FormsForMainMenu
    {
        public List<string> GetVideoPath()
        {
            string filePath = "1 ";
            string txtFilePath = " 2 ";
            var Pahtslist = new List<string>();

            Form inputForm = new Form();
            //inputForm.Text = "Enter File Path";
            //inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            //inputForm.MaximizeBox = false;
            //inputForm.MinimizeBox = false;
            //inputForm.StartPosition = FormStartPosition.CenterScreen;

            //// Добавление текстового поля
            //TextBox textBox = new TextBox();
            //textBox.Location = new System.Drawing.Point(10, 10);
            //textBox.Size = new System.Drawing.Size(250, 20);
            //inputForm.Controls.Add(textBox);

            //TextBox txtFileTextBox = new TextBox();
            //txtFileTextBox.Location = new System.Drawing.Point(10, 60);
            //txtFileTextBox.Size = new System.Drawing.Size(250, 20);
            //inputForm.Controls.Add(txtFileTextBox);


            // кнопка Ок
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new System.Drawing.Point(90, 100);
            inputForm.Controls.Add(okButton);

            // Визуализация для юзера
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                // получение пути из текст бокса
                //filePath = textBox.Text;
                //txtFilePath = txtFileTextBox.Text;


                MessageBox.Show("Немного подождите, генерируются субтитры");
               // MessageBox.Show("File Name: " + txtFileTextBox);
            }
            //Pahtslist[0] = filePath;
           // Pahtslist[1] = txtFilePath;
            Pahtslist.Insert(0, filePath);
            Pahtslist.Insert(1,txtFilePath);
            //File.WriteAllText("E:\\Visual_studio_files_and_Visual_trash\\TestProj\\TestProj\\testing.txt", filePath);
            return Pahtslist;
        }
    }
}
