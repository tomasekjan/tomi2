using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Collections;
using GraphEditor.Exceptions;
using System.Windows;

namespace GraphEditor.GraphDeclaration
{
    public sealed class UndoRedeSerializer
    {
        const int STACK_SIZE = 20;
        const int AUTO_SAVE_COUNT = 10;
        Guid guid;
        int currentIndex;
        int UndoCout;
        int ActionCout;
        MemoryStream[] stack;        
        BinaryFormatter binaryFormater;
        FileInfo autoSaveFile;
        object key;
        bool inicialized = false;

        public UndoRedeSerializer()
        {
            guid = Guid.NewGuid();
            
            currentIndex = 0;
            UndoCout = 0;
            stack = new MemoryStream[STACK_SIZE];
            binaryFormater = new BinaryFormatter();
            for (int i = 0; i < STACK_SIZE; i++)
            {
                stack[i] = new MemoryStream();
            }
            key = new object();
        }

        /// <summary>
        /// initialize auto save function
        /// </summary>
        private void InicializeAutoSave()
        {
            DirectoryInfo dir = new DirectoryInfo("AutoSave");
            if (!dir.Exists)
            {
                dir.Create();
            }
            autoSaveFile = new FileInfo(String.Format("AutoSave\\{1}-{2}-{3}-{0}.grf", guid,DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));            
        }

        /// <summary>
        /// Stores current graph configuration to memory stream on stack
        /// </summary>
        /// <param name="graphDeclaration">current graph configuration</param>
        public void Action(GraphDefinition graphDeclaration)
        {
            lock (key)
            {
                
                stack[currentIndex % STACK_SIZE] = new MemoryStream();
                binaryFormater.Serialize(stack[currentIndex % STACK_SIZE], graphDeclaration);
                stack[currentIndex % STACK_SIZE].Flush();
                ActionCout++;
                currentIndex++;
                if (ActionCout % AUTO_SAVE_COUNT == 0 && ActionCout != 0)
                {
                    AutoSaveToFile(graphDeclaration);
                }
            }
        }

        /// <summary>
        /// save to file 
        /// </summary>
        /// <param name="graphDeclaration"></param>
        private void AutoSaveToFile(GraphDefinition graphDeclaration)
        {
            if (!inicialized)
            {
                InicializeAutoSave();
                inicialized = true;
            }
            using (FileStream fileStream = new FileStream(autoSaveFile.FullName, FileMode.Create))
            {
                binaryFormater.Serialize(fileStream, graphDeclaration);
                fileStream.Close();
            }
        }

        /// <summary>
        /// returns last saved configuration from stack
        /// </summary>
        /// <returns></returns>
        public GraphDefinition Undo()
        {
            lock (key)
            {
                GraphDefinition graphDeclaration = null;
                if ((ActionCout - UndoCout > 0) && (UndoCout < ActionCout))
                {
                    stack[(currentIndex + STACK_SIZE - 2) % STACK_SIZE].Seek(0, SeekOrigin.Begin);
                    graphDeclaration = (GraphDefinition)binaryFormater.Deserialize(stack[(currentIndex + STACK_SIZE - 2) % STACK_SIZE]);
                    UndoCout++;
                    currentIndex -= 2;
                }                
                return graphDeclaration;
            }
        }

        /// <summary>
        /// return of return :D
        /// </summary>
        /// <returns></returns>
        public GraphDefinition Redo()
        {
            lock (key)
            {
                GraphDefinition graphDeclaration = null;

                if (stack[(currentIndex + 1) % STACK_SIZE].Length != 0)
                {
                    stack[(currentIndex + 1) % STACK_SIZE].Seek(0, SeekOrigin.Begin);
                    graphDeclaration = (GraphDefinition)binaryFormater.Deserialize(stack[(currentIndex + 1) % STACK_SIZE]);
                    UndoCout--;
                    currentIndex++;
                }
                return graphDeclaration;
            }
        }

        /// <summary>
        /// graph serialization
        /// </summary>
        /// <param name="fileName">target file name</param>
        /// <param name="graph">graph object to serialize</param>  
        /// <exception cref="SerializationException">throws then there is some IO error by serializing</exception>
        public static void Serialize(string fileName, Graph graph)
        {
            try
            {
                Stream stream = File.Open(fileName, FileMode.Create);
                BinaryFormatter binaryFormater = new BinaryFormatter();
                binaryFormater.Serialize(stream, graph.graphDeclaration);
            }
            catch (Exception exception)
            {
                throw new SerializationException("Error by serializing", exception);
            }
        }

        /// <summary>
        /// graph de serialization
        /// </summary>
        /// <param name="fileName">source fine name</param>
        /// <returns>de serialized graph</returns>
        /// <exception cref="SerializationException">throws then there is some IO error by serializing</exception>
        public static GraphDefinition Deserialize(string fileName)
        {
            try
            {
                Stream stream = File.Open(fileName, FileMode.Open);
                BinaryFormatter binaryFormater = new BinaryFormatter();
                return (GraphDefinition)binaryFormater.Deserialize(stream);
            }
            catch (Exception exception)
            {
                throw new SerializationException("Error by serializing", exception);
            }
        }
    }
}
