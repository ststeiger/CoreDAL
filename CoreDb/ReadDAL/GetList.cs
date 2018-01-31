
namespace CoreDb
{
    partial class ReadDAL
    {


        // Anything else than a struct or a class
        public virtual bool IsSimpleType(System.Type tThisType)
        {
            
            if (System.Reflection.IntrospectionExtensions.GetTypeInfo(tThisType).IsPrimitive)
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.String)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.DateTime)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Guid)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Decimal)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Object)))
            {
                return true;
            }

            return false;
        } // End Function IsSimpleType



        public virtual System.Collections.Generic.List<T> GetList<T>(System.Data.IDbCommand cmd)
        {
            System.Collections.Generic.List<T> lsReturnValue = new System.Collections.Generic.List<T>();
            T tThisValue = default(T);
            System.Type tThisType = typeof(T);

            using (System.Data.IDataReader idr = ExecuteReader(cmd))
            {

                if (IsSimpleType(tThisType))
                {
                    while (idr.Read())
                    {
                        object objVal = idr.GetValue(0);

                        // tThisValue = System.Convert.ChangeType(objVal, T),
                        tThisValue = (T)ConvertResult<T>(objVal, true);

                        lsReturnValue.Add(tThisValue);
                    } // End while (idr.Read())

                }
                else
                {
                    int iFieldCount = idr.FieldCount;
                    Setter_t<T>[] mems = new Setter_t<T>[iFieldCount];

                    for (int i = 0; i < iFieldCount; ++i)
                    {
                        string strName = idr.GetName(i);
                        mems[i] = LinqHelper.GetSetter<T>(strName);
                    } // Next i


                    while (idr.Read())
                    {
                        tThisValue = System.Activator.CreateInstance<T>();

                        for (int i = 0; i < iFieldCount; ++i)
                        {
                            Setter_t<T> setter = mems[i];

                            if (setter != null)
                            {
                                object objVal = idr.GetValue(i);
                                setter(tThisValue, objVal);
                            }

                        } // Next i

                        lsReturnValue.Add(tThisValue);
                    } // Whend

                } // End if IsSimpleType(tThisType)

                idr.Close();
            } // End Using idr

            return lsReturnValue;
        } // End Function GetList


        public virtual System.Collections.Generic.List<T> GetList<T>(string strSQL)
        {
            System.Collections.Generic.List<T> lsReturnValue = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strSQL))
            {
                lsReturnValue = GetList<T>(cmd);
            } // End Using cmd

            return lsReturnValue;
        } // End Function GetList


    }


}
