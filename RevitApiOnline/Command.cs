using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitApiOnline.DataGridLearn;
using RevitApiOnline.ListBox;
using System.Collections.Generic;
using System.Linq;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            #region test
            //var familyCollection = new FilteredElementCollector(doc).OfClass(typeof(Family))
            //    .Cast<Family>().Where(x => x.FamilyCategoryId.Value == (long)BuiltInCategory.OST_StructuralFraming)
            //    .ToList();
            //DataGridAppShow.ListFamilyBeam = familyCollection;

            //List<DataGridItem> listDataGrid = new List<DataGridItem>();

            //List<BeamFamilyVM> listBeamFamilyVm = familyCollection
            //    .Select(x => new BeamFamilyVM { FamilyId = x.Id, FamilyName = x.Name }).ToList();
            ////foreach(Family family in familyCollection)
            ////{
            ////    BeamFamilyVM beamFamilyVM = new BeamFamilyVM();
            ////    beamFamilyVM.FamilyName = family.Name;
            ////    beamFamilyVM.FamilyId = family.Id;
            ////    listBeamFamilyVm.Add(beamFamilyVM);
            ////}

            //foreach (Family family in familyCollection)
            //{
            //    DataGridItem dataItem = new DataGridItem();
            //    dataItem.BeamFamilies = listBeamFamilyVm;
            //    listDataGrid.Add(dataItem);
            //}
            //List<TreeViewItemVm> listTreeViewItem = new List<TreeViewItemVm>();
            //foreach (Category category in doc.Settings.Categories)
            //{
            //    TreeViewItemVm treeViewItem = new TreeViewItemVm();
            //    treeViewItem.Name= category.Name;
            //    treeViewItem.Id= category.Id;

            //    var familyCollectionItem = new FilteredElementCollector(doc).OfClass(typeof(Family))
            //   .Cast<Family>().Where(x => x.FamilyCategoryId == category.Id)
            //   .ToList();
            //    foreach(Family family in familyCollectionItem)
            //    {
            //        TreeViewItemVm treeViewItemFamily = new TreeViewItemVm();
            //        treeViewItemFamily.Name= family.Name;
            //        treeViewItemFamily.Id= family.Id;
            //        treeViewItem.Items.Add(treeViewItemFamily);
            //        var symbolIds = family.GetFamilySymbolIds();
            //        if (symbolIds != null)
            //        {
            //            foreach(ElementId idSy in family.GetFamilySymbolIds())
            //            {
            //                FamilySymbol familySym = doc.GetElement(idSy) as FamilySymbol;
            //                TreeViewItemVm treeViewItemSymbol = new TreeViewItemVm();
            //                treeViewItemSymbol.Name= familySym.Name;
            //                treeViewItemSymbol.Id= familySym.Id;
            //                treeViewItemFamily.Items.Add(treeViewItemSymbol);
            //            }
            //        }
            //        treeViewItem.Items.Add(treeViewItemFamily);
            //    }
            //    if(treeViewItem.Items.Count > 0)
            //    {
            //        listTreeViewItem.Add(treeViewItem);
            //    }
            //}

            //var form = new DataGridWpf(doc, listTreeViewItem);
            //form.dataGridFamilyBeam.ItemsSource = listDataGrid;
            //bool? resultForm= form.ShowDialog();
            //if (resultForm == true)
            //{
            //    List<DataGridItem> listResultDataGrid = form.dataGridFamilyBeam.ItemsSource as List<DataGridItem>;

            //}
            #endregion

            ///
            /// 
            /// Geometry
            /// 
            ElementId selectedId= uiDoc.Selection.GetElementIds().FirstOrDefault();
            Element element = doc.GetElement(selectedId);

            Options geoOption = new Options();
            geoOption.IncludeNonVisibleObjects = false;
            geoOption.DetailLevel = ViewDetailLevel.Medium;
            geoOption.ComputeReferences = true;
            //geoOption.View = doc.ActiveView;

            GeometryResult geometryResult= new GeometryResult();
            GeometryElement geometryElement = element.get_Geometry(geoOption);
            foreach(GeometryObject geoObj in geometryElement)
            {
                if(geoObj !=null && geoObj is Solid)
                {
                    Solid solid = geoObj as Solid;
                    if(solid.Volume> 0.00000001)
                    {
                        geometryResult.Solids.Add(solid);
                        XYZ centroilPoit= solid.ComputeCentroid();
                        foreach(Face face in solid.Faces)
                        {
                            geometryResult.Faces.Add(face);
                            PlanarFace plannarFace= face as PlanarFace;
                            if(plannarFace != null)
                            {
                                XYZ originFace = plannarFace.Origin;
                                XYZ originUV = plannarFace.XVector;
                                UV uv = new UV(0.5, 0.5);
                                XYZ point = face.Evaluate(uv);
                                XYZ normalFace = plannarFace.FaceNormal.Normalize();
                            }
                            else 
                            {
                                UV uv = new UV(0.5, 0.5);
                                Transform transform= face.ComputeDerivatives(uv);
                                XYZ tiepTuyen= transform.BasisX.Normalize();
                                XYZ phap = transform.BasisY.Normalize();
                            }

                            double area= face.Area;
                            List<CurveLoop> listCurveloop = new List<CurveLoop>();
                            foreach(CurveLoop curveloop in face.GetEdgesAsCurveLoops())
                            {
                                listCurveloop.Add(curveloop);
                            }

                            foreach(EdgeArray edges in face.EdgeLoops)
                            {
                                foreach(Edge edge in edges)
                                {
                                    geometryResult.Edges.Add(edge);
                                    Curve curve = edge.AsCurve();


                                }
                            }

                        }
                    }
                }
                else if(geoObj is Curve)
                {
                    geometryResult.Curves.Add(geoObj as Curve);
                }
                else if(geoObj is GeometryInstance)
                {
                    GeometryInstance geoInstance = geoObj as GeometryInstance;
                    var geoElementsInstace = geoInstance.GetInstanceGeometry();
                    foreach(GeometryObject geoObj2 in geoElementsInstace)
                    {

                    }
                    var geoSymbol = geoInstance.GetSymbolGeometry();
                    Transform transformInsace = geoInstance.Transform;
                    foreach(GeometryObject geObj3 in geoSymbol)
                    {
                        Solid solid2 = geObj3 as Solid;
                        foreach(Face face in solid2.Faces)
                        {
                            PlanarFace plaanerface = face as PlanarFace;
                            if (plaanerface != null)
                            {
                                XYZ normal = plaanerface.FaceNormal.Normalize();
                                XYZ originFa = plaanerface.Origin;
                                XYZ noramlProject = transformInsace.OfVector(normal);
                                XYZ originProject = transformInsace.OfPoint(originFa);
                            }
                        }
                    }
                }
                
            }

            Transform transformOrigin = Transform.Identity;
            transformOrigin.BasisX = new XYZ(1, 0, 0);
            transformOrigin.BasisY = new XYZ(0, 1, 0);
            transformOrigin.BasisZ = new XYZ(0, 0, 1);

            transformOrigin.Origin = XYZ.Zero;





            return Result.Succeeded;
        }
    }

    
    public class GeometryResult
    {
        public GeometryResult()
        {
            Solids= new List<Solid> ();
            Faces = new List<Face>();
            Edges= new List<Edge>();    
            Curves= new List<Curve>();    
        }
        public List< Solid> Solids { set; get; }

        public List<Face> Faces { set; get; }

        public List<Edge> Edges { set; get; }

        public List<Curve> Curves { set; get; }
       
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem != null && elem is Wall)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}