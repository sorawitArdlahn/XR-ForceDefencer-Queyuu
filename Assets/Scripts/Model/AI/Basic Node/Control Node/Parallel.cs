 namespace AI.Basic_Node.Control_Node
 {
     public class Parallel : Node
     {
         //ลองดูก่อนนะว่าทำงานได้ไหม
         private int successCount;
         private int failureCount;

         public Parallel(string name, int priority = 0) : base(name, priority) {}

         public override Status Process()
         {
             successCount = 0;
             failureCount = 0;

             foreach (var child in children)
             {
                 var status = child.Process();

                 switch (status)
                 {
                     case Status.Success:
                         successCount++;
                         break;
                     case Status.Failure:
                         Reset();
                         return Status.Failure; // ถ้า child ใดล้มเหลว คืน Failure ทันที
                     case Status.Running:
                         // Do nothing; รอให้ node ทั้งหมดประมวลผลเสร็จ
                         break;
                 }
             }

             if (successCount == children.Count) // ถ้าทุก child สำเร็จ
             {
                 Reset();
                 return Status.Success;
             }

             return Status.Running; // ถ้ายังมี node ที่กำลังทำงาน
         }
     }
 }
