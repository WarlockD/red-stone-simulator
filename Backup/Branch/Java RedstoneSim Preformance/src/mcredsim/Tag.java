
package mcredsim;

import java.io.*;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

public class Tag
{
    public enum Type
    {
        TAG_End, 
        TAG_Byte,
        TAG_Short, 
        TAG_Int, 
        TAG_Long, 
        TAG_Float, 
        TAG_Double, 
        TAG_Byte_Array,
        TAG_String, 
        TAG_List,
        TAG_Compound;
    }
      


    public Tag(Type type, String name, Tag value[])
    {
        listType = null;
        this.type = type;
        this.name = name;
        setValue(value);
    }

    public Tag(String name, Type listType)
    {
        this(Type.TAG_List, name, listType);
    }

    public Tag(Type type, String name, Object value)
    {
        listType = null;
        this.type = type;
        this.name = name;
        setValue(value);
    }

    public void setBAValue(byte value[])
    {
        setValue(value);
    }

    public final void setValue(Object value)
    {
        if(type == Type.TAG_Compound && !(value instanceof Tag[]))
            throw new IllegalArgumentException();
        switch(type)
        {
        case TAG_End: 
            if(value != null)
                throw new IllegalArgumentException();
            break;

        case TAG_Byte: 
            if(!(value instanceof Byte))
                throw new IllegalArgumentException();
            break;

        case TAG_Short: 
            if(!(value instanceof Short))
                throw new IllegalArgumentException();
            break;

        case TAG_Int: 
            if(!(value instanceof Integer))
                throw new IllegalArgumentException();
            break;

        case TAG_Long: 
            if(!(value instanceof Long))
                throw new IllegalArgumentException();
            break;

        case TAG_Float:
            if(!(value instanceof Float))
                throw new IllegalArgumentException();
            break;

        case TAG_Double: 
            if(!(value instanceof Double))
                throw new IllegalArgumentException();
            break;

        case TAG_Byte_Array:
            if(!(value instanceof byte[]))
                throw new IllegalArgumentException();
            break;

        case TAG_String: 
            if(!(value instanceof String))
                throw new IllegalArgumentException();
            break;

        case TAG_List: 
            if(value instanceof Type)
            {
                listType = (Type)value;
                value = new Tag[0];
                break;
            }
            if(!(value instanceof Tag[]))
                throw new IllegalArgumentException();
            listType = ((Tag[])value)[0].getType();
            break;

        case TAG_Compound: 
            if(!(value instanceof Tag[]))
                throw new IllegalArgumentException();
            break;

        default:
            throw new IllegalArgumentException();
        }
        this.value = value;
    }

    public Type getType()
    {
        return type;
    }

    public String getName()
    {
        return name;
    }

    public Object getValue()
    {
        return value;
    }

    public Type getListType()
    {
        return listType;
    }

    public void addTag(Tag tag)
    {
        if(type != Type.TAG_List && type != Type.TAG_Compound)
        {
            throw new RuntimeException();
        } else
        {
            Tag subtags[] = (Tag[])value;
            insertTag(tag, subtags.length);
            return;
        }
    }

    public void insertTag(Tag tag, int index)
    {
        if(type != Type.TAG_List && type != Type.TAG_Compound)
            throw new RuntimeException();
        Tag subtags[] = (Tag[])value;
        if(subtags.length > 0 && type == Type.TAG_List && tag.getType() != getListType())
            throw new IllegalArgumentException();
        if(index > subtags.length)
        {
            throw new IndexOutOfBoundsException();
        } else
        {
            Tag newValue[] = new Tag[subtags.length + 1];
            System.arraycopy(subtags, 0, newValue, 0, index);
            newValue[index] = tag;
            System.arraycopy(subtags, index, newValue, index + 1, subtags.length - index);
            value = newValue;
            return;
        }
    }

    public Tag removeTag(int index)
    {
        if(type != Type.TAG_List && type != Type.TAG_Compound)
        {
            throw new RuntimeException();
        } else
        {
            Tag subtags[] = (Tag[])value;
            Tag victim = subtags[index];
            Tag newValue[] = new Tag[subtags.length - 1];
            System.arraycopy(subtags, 0, newValue, 0, index);
            index++;
            System.arraycopy(subtags, index, newValue, index - 1, subtags.length - index);
            value = newValue;
            return victim;
        }
    }

    public void removeSubTag(Tag tag)
    {
        if(type != Type.TAG_List && type != Type.TAG_Compound)
            throw new RuntimeException();
        if(tag == null)
            return;
        Tag subtags[] = (Tag[])value;
        for(int i = 0; i < subtags.length; i++)
        {
            if(subtags[i] == tag)
            {
                removeTag(i);
                return;
            }
            if(subtags[i].type == Type.TAG_List || subtags[i].type == Type.TAG_Compound)
                subtags[i].removeSubTag(tag);
        }

    }

    public Tag findTagByName(String name)
    {
        return findNextTagByName(name, null);
    }

    public Tag findNextTagByName(String name, Tag found)
    {
        if(type != Type.TAG_List && type != Type.TAG_Compound)
            return null;
        Tag subtags[] = (Tag[])value;
        Tag atag[];
        int j = (atag = subtags).length;
        for(int i = 0; i < j; i++)
        {
            Tag subtag = atag[i];
            if(subtag.name == null && name == null || subtag.name != null && subtag.name.equals(name))
                return subtag;
            Tag newFound = subtag.findTagByName(name);
            if(newFound != null && newFound != found)
                return newFound;
        }

        return null;
    }

    public static Tag readFrom(InputStream is)
        throws IOException
    {
        DataInputStream dis = new DataInputStream(new GZIPInputStream(is));
        byte type = dis.readByte();
        if(type == 0)
            return new Tag(Type.TAG_End, null, ((Tag []) (null)));
        else
            return new Tag(Type.values()[type], dis.readUTF(), readPayload(dis, type));
    }

    private static Object readPayload(DataInputStream dis, byte type)
        throws IOException
    {
        switch(type)
        {
        case 0: 
            return null;

        case 1: 
            return Byte.valueOf(dis.readByte());

        case 2: 
            return Short.valueOf(dis.readShort());

        case 3: 
            return Integer.valueOf(dis.readInt());

        case 4: 
            return Long.valueOf(dis.readLong());

        case 5: 
            return Float.valueOf(dis.readFloat());

        case 6: 
            return Double.valueOf(dis.readDouble());

        case 7: 
            int length = dis.readInt();
            byte ba[] = new byte[length];
            dis.readFully(ba);
            return ba;

        case 8: 
            return dis.readUTF();

        case 9: 
            byte lt = dis.readByte();
            int ll = dis.readInt();
            Tag lo[] = new Tag[ll];
            for(int i = 0; i < ll; i++)
                lo[i] = new Tag(Type.values()[lt], null, readPayload(dis, lt));

            if(lo.length == 0)
                return Type.values()[lt];
            else
                return lo;

        case 10: 
            Tag tags[] = new Tag[0];
            byte stt;
            do
            {
                stt = dis.readByte();
                String name = null;
                if(stt != 0)
                    name = dis.readUTF();
                Tag newTags[] = new Tag[tags.length + 1];
                System.arraycopy(tags, 0, newTags, 0, tags.length);
                newTags[tags.length] = new Tag(Type.values()[stt], name, readPayload(dis, stt));
                tags = newTags;
            } while(stt != 0);
            return tags;
        }
        return null;
    }

    public void writeTo(OutputStream os)
        throws IOException
    {
        GZIPOutputStream gzos;
        DataOutputStream dos = new DataOutputStream(gzos = new GZIPOutputStream(os));
        dos.writeByte(type.ordinal());
        if(type != Type.TAG_End)
        {
            dos.writeUTF(name);
            writePayload(dos);
        }
        gzos.flush();
        gzos.close();
    }

    private void writePayload(DataOutputStream dos)
        throws IOException
    {
        switch(this.type)
        {
        case TAG_End: 
        default:
            break;

        case TAG_Byte: 
            dos.writeByte(((Byte)value).byteValue());
            break;

        case TAG_Short:
            dos.writeShort(((Short)value).shortValue());
            break;

        case TAG_Int:
            dos.writeInt(((Integer)value).intValue());
            break;

        case TAG_Long: 
            dos.writeLong(((Long)value).longValue());
            break;

        case TAG_Float: 
            dos.writeFloat(((Float)value).floatValue());
            break;

        case TAG_Double: 
            dos.writeDouble(((Double)value).doubleValue());
            break;

        case TAG_Byte_Array: 
            byte ba[] = (byte[])value;
            dos.writeInt(ba.length);
            dos.write(ba);
            break;

        case TAG_String: 
            dos.writeUTF((String)value);
            break;

        case TAG_List: 
            Tag list[] = (Tag[])value;
            dos.writeByte(getListType().ordinal());
            dos.writeInt(list.length);
            Tag atag[];
            int j = (atag = list).length;
            for(int i = 0; i < j; i++)
            {
                Tag tt = atag[i];
                tt.writePayload(dos);
            }

            break;

        case TAG_Compound: 
            Tag subtags[] = (Tag[])value;
            Tag atag1[];
            int l = (atag1 = subtags).length;
            for(int k = 0; k < l; k++)
            {
                Tag st = atag1[k];
                Tag subtag = st;
                Type type = subtag.getType();
                dos.writeByte(type.ordinal());
                if(type != Type.TAG_End)
                {
                    dos.writeUTF(subtag.getName());
                    subtag.writePayload(dos);
                }
            }

            break;
        }
    }

    public void print()
    {
        print(this, 0);
    }

    private String getTypeString(Type type)
    {
        switch(type)
        {
        case TAG_End: 
            return "TAG_End";

        case TAG_Byte: 
            return "TAG_Byte";

        case TAG_Short:
            return "TAG_Short";

        case TAG_Int: 
            return "TAG_Int";

        case TAG_Long: 
            return "TAG_Long";

        case TAG_Float: 
            return "TAG_Float";

        case TAG_Double:
            return "TAG_Double";

        case TAG_Byte_Array: 
            return "TAG_Byte_Array";

        case TAG_String: 
            return "TAG_String";

        case TAG_List: 
            return "TAG_List";

        case TAG_Compound: 
            return "TAG_Compound";
        }
        return null;
    }

    private void indent(int indent)
    {
        for(int i = 0; i < indent; i++)
            System.out.print("   ");

    }

    private void print(Tag t, int indent)
    {
        Type type = t.getType();
        if(type == Type.TAG_End)
            return;
        String name = t.getName();
        indent(indent);
        System.out.print(getTypeString(t.getType()));
        if(name != null)
            System.out.print((new StringBuilder("(\"")).append(t.getName()).append("\")").toString());
        if(type == Type.TAG_Byte_Array)
        {
            byte b[] = (byte[])t.getValue();
            System.out.println((new StringBuilder(": [")).append(b.length).append(" bytes]").toString());
        } else
        if(type == Type.TAG_List)
        {
            Tag subtags[] = (Tag[])t.getValue();
            System.out.println((new StringBuilder(": ")).append(subtags.length).append(" entries of type ").append(getTypeString(t.getListType())).toString());
            Tag atag[];
            int k = (atag = subtags).length;
            for(int i = 0; i < k; i++)
            {
                Tag st = atag[i];
                print(st, indent + 1);
            }

            indent(indent);
            System.out.println("}");
        } else
        if(type == Type.TAG_Compound)
        {
            Tag subtags[] = (Tag[])t.getValue();
            System.out.println((new StringBuilder(": ")).append(subtags.length - 1).append(" entries").toString());
            indent(indent);
            System.out.println("{");
            Tag atag1[];
            int l = (atag1 = subtags).length;
            for(int j = 0; j < l; j++)
            {
                Tag st = atag1[j];
                print(st, indent + 1);
            }

            indent(indent);
            System.out.println("}");
        } else
        {
            System.out.println((new StringBuilder(": ")).append(t.getValue()).toString());
        }
    }


   
    private final Type type;
    private Type listType;
    private final String name;
    private Object value;

}