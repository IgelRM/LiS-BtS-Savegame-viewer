using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;

namespace savefiledecoder
{
    public class AssFile
    {
        ModuleDefinition module;

        string AssPath;

        public AssFile(string path)
        {
            AssPath = path;
        }

        public void Read()
        {
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(Directory.GetParent(AssPath).ToString());
            module = ModuleDefinition.ReadModule(AssPath, new ReaderParameters() { AssemblyResolver = resolver });
        }

        public bool? CheckIntroSkip()
        {
            TypeDefinition typeDef = module.GetType("T_EDB11480");
            MethodDefinition method = typeDef.Methods[0];
            foreach (MethodDefinition met in typeDef.Methods)
            {
                if (met.Name == "_17E2DB9F5")
                {
                    method = met;
                    break;
                }
            }

            if (method.Body.Instructions.Count == 12) return false;
            else if (method.Body.Instructions.Count == 4) return true;
            else return null;
        }

        public bool? CheckCutsceneSkip()
        {
            TypeDefinition typeDef = module.GetType("T_BF5A5EEC");
            PropertyDefinition prop = typeDef.Properties[0];
            foreach (PropertyDefinition pr in typeDef.Properties)
            {
                if (pr.Name == "_17CBB7FF8")
                {
                    prop = pr;
                    break;
                }
            }

            if (prop.GetMethod.Body.Instructions[13].OpCode == OpCodes.Ldc_I4_0) return false;
            else if (prop.GetMethod.Body.Instructions[13].OpCode == OpCodes.Ldc_I4_1) return true;
            else return null;
        }

        public void ChangeIntroSkip(bool enable)
        {
            TypeDefinition typeDef = module.GetType("T_EDB11480");
            MethodDefinition method = typeDef.Methods[0];
            foreach (MethodDefinition met in typeDef.Methods)
            {
                if (met.Name == "_17E2DB9F5")
                {
                    method = met;
                    break;
                }
            }
            ILProcessor ilprocessor = method.Body.GetILProcessor();

            if (enable && method.Body.Instructions.Count == 12)
            {
                for (int i = 0; i < 8; i++)
                {
                    ilprocessor.Remove(method.Body.Instructions.First());
                }
            }

            else if (method.Body.Instructions.Count == 4)
            {
                List<Instruction> instr_list = new List<Instruction>();

                instr_list.Add(ilprocessor.Create(OpCodes.Ldarg_0));

                FieldDefinition splashlist = typeDef.Fields[0];
                foreach (FieldDefinition fld in typeDef.Fields)
                {
                    if (fld.Name == "m_splashList")
                    {
                        splashlist = fld;
                        break;
                    }
                }
                instr_list.Add(ilprocessor.Create(OpCodes.Ldfld, splashlist));

                MethodReference get_count = module.Import(typeof(Queue<>).GetProperty("Count").GetGetMethod());
                instr_list.Add(ilprocessor.Create(OpCodes.Callvirt, get_count));

                //skipping instruction 3 for now

                instr_list.Add(ilprocessor.Create(OpCodes.Ldarg_0));
                instr_list.Add(ilprocessor.Create(OpCodes.Ldfld, splashlist));

                MethodReference dequeue = module.Import(typeof(Queue<>).GetMethod("Dequeue"));
                instr_list.Add(ilprocessor.Create(OpCodes.Callvirt, dequeue));

                instr_list.Add(ilprocessor.Create(OpCodes.Ret));

                ((MethodReference)instr_list[2].Operand).DeclaringType = ((FieldReference)instr_list[1].Operand).FieldType;
                ((MethodReference)instr_list[5].Operand).DeclaringType = ((FieldReference)instr_list[4].Operand).FieldType;

                instr_list.Reverse();

                foreach (Instruction instr in instr_list)
                {
                    ilprocessor.InsertBefore(method.Body.Instructions.First(), instr);
                }
                ilprocessor.InsertBefore(method.Body.Instructions[3], ilprocessor.Create(OpCodes.Brfalse, method.Body.Instructions[7]));
            }
        }

        public void ChangeCutsceneSkip(bool enable)
        {
            TypeDefinition typeDef = module.GetType("T_BF5A5EEC");
            PropertyDefinition prop = typeDef.Properties[0];
            foreach (PropertyDefinition pr in typeDef.Properties)
            {
                if (pr.Name == "_17CBB7FF8")
                {
                    prop = pr;
                    break;
                }
            }

            if (enable)
            {
                prop.GetMethod.Body.Instructions[13].OpCode = OpCodes.Ldc_I4_1;
            }
            else
            {
                prop.GetMethod.Body.Instructions[13].OpCode = OpCodes.Ldc_I4_0;
            }
        }

        public bool Write ()
        {
            try
            {
                if (!File.Exists(AssPath + @".bkp"))
                {
                    File.Copy(AssPath, AssPath + @".bkp", false);
                }

                module.Write(AssPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
