// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   Tag.java

package com.carneiro.mcredsim;

import java.io.*;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

public class Tag
{
    public static final class Type extends Enum
    {

        public static Type[] values()
        {
            Type atype[];
            int i;
            Type atype1[];
            System.arraycopy(atype = ENUM$VALUES, 0, atype1 = new Type[i = atype.length], 0, i);
            return atype1;
        }

        public static Type valueOf(String s)
        {
            return (Type)Enum.valueOf(com/carneiro/mcredsim/Tag$Type, s);
        }

        public static final Type TAG_End;
        public static final Type TAG_Byte;
        public static final Type TAG_Short;
        public static final Type TAG_Int;
        public static final Type TAG_Long;
        public static final Type TAG_Float;
        public static final Type TAG_Double;
        public static final Type TAG_Byte_Array;
        public static final Type TAG_String;
        public static final Type TAG_List;
        public static final Type TAG_Compound;
        private static final Type ENUM$VALUES[];

        static 
        {
            TAG_End = new Type("TAG_End", 0);
            TAG_Byte = new Type("TAG_Byte", 1);
            TAG_Short = new Type("TAG_Short", 2);
            TAG_Int = new Type("TAG_Int", 3);
            TAG_Long = new Type("TAG_Long", 4);
            TAG_Float = new Type("TAG_Float", 5);
            TAG_Double = new Type("TAG_Double", 6);
            TAG_Byte_Array = new Type("TAG_Byte_Array", 7);
            TAG_String = new Type("TAG_String", 8);
            TAG_List = new Type("TAG_List", 9);
            TAG_Compound = new Type("TAG_Compound", 10);
            ENUM$VALUES = (new Type[] {
                TAG_End, TAG_Byte, TAG_Short, TAG_Int, TAG_Long, TAG_Float, TAG_Double, TAG_Byte_Array, TAG_String, TAG_List, 
                TAG_Compound
            });
        }

        private Type(String s, int i)
        {
            super(s, i);
        }
    }


    public Tag(Type type, String name, Tag value[])
    {
        this(type, name, value);
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

    public void setValue(Object value)
    {
        if(type == Type.TAG_Compound && !(value instanceof Tag[]))
            throw new IllegalArgumentException();
        switch($SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type()[type.ordinal()])
        {
        case 1: // '\001'
            if(value != null)
                throw new IllegalArgumentException();
            break;

        case 2: // '\002'
            if(!(value instanceof Byte))
                throw new IllegalArgumentException();
            break;

        case 3: // '\003'
            if(!(value instanceof Short))
                throw new IllegalArgumentException();
            break;

        case 4: // '\004'
            if(!(value instanceof Integer))
                throw new IllegalArgumentException();
            break;

        case 5: // '\005'
            if(!(value instanceof Long))
                throw new IllegalArgumentException();
            break;

        case 6: // '\006'
            if(!(value instanceof Float))
                throw new IllegalArgumentException();
            break;

        case 7: // '\007'
            if(!(value instanceof Double))
                throw new IllegalArgumentException();
            break;

        case 8: // '\b'
            if(!(value instanceof byte[]))
                throw new IllegalArgumentException();
            break;

        case 9: // '\t'
            if(!(value instanceof String))
                throw new IllegalArgumentException();
            break;

        case 10: // '\n'
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

        case 11: // '\013'
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
        case 0: // '\0'
            return null;

        case 1: // '\001'
            return Byte.valueOf(dis.readByte());

        case 2: // '\002'
            return Short.valueOf(dis.readShort());

        case 3: // '\003'
            return Integer.valueOf(dis.readInt());

        case 4: // '\004'
            return Long.valueOf(dis.readLong());

        case 5: // '\005'
            return Float.valueOf(dis.readFloat());

        case 6: // '\006'
            return Double.valueOf(dis.readDouble());

        case 7: // '\007'
            int length = dis.readInt();
            byte ba[] = new byte[length];
            dis.readFully(ba);
            return ba;

        case 8: // '\b'
            return dis.readUTF();

        case 9: // '\t'
            byte lt = dis.readByte();
            int ll = dis.readInt();
            Tag lo[] = new Tag[ll];
            for(int i = 0; i < ll; i++)
                lo[i] = new Tag(Type.values()[lt], null, readPayload(dis, lt));

            if(lo.length == 0)
                return Type.values()[lt];
            else
                return lo;

        case 10: // '\n'
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
        switch($SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type()[this.type.ordinal()])
        {
        case 1: // '\001'
        default:
            break;

        case 2: // '\002'
            dos.writeByte(((Byte)value).byteValue());
            break;

        case 3: // '\003'
            dos.writeShort(((Short)value).shortValue());
            break;

        case 4: // '\004'
            dos.writeInt(((Integer)value).intValue());
            break;

        case 5: // '\005'
            dos.writeLong(((Long)value).longValue());
            break;

        case 6: // '\006'
            dos.writeFloat(((Float)value).floatValue());
            break;

        case 7: // '\007'
            dos.writeDouble(((Double)value).doubleValue());
            break;

        case 8: // '\b'
            byte ba[] = (byte[])value;
            dos.writeInt(ba.length);
            dos.write(ba);
            break;

        case 9: // '\t'
            dos.writeUTF((String)value);
            break;

        case 10: // '\n'
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

        case 11: // '\013'
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
        switch($SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type()[type.ordinal()])
        {
        case 1: // '\001'
            return "TAG_End";

        case 2: // '\002'
            return "TAG_Byte";

        case 3: // '\003'
            return "TAG_Short";

        case 4: // '\004'
            return "TAG_Int";

        case 5: // '\005'
            return "TAG_Long";

        case 6: // '\006'
            return "TAG_Float";

        case 7: // '\007'
            return "TAG_Double";

        case 8: // '\b'
            return "TAG_Byte_Array";

        case 9: // '\t'
            return "TAG_String";

        case 10: // '\n'
            return "TAG_List";

        case 11: // '\013'
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

    public static void main(String args[])
    {
        throw new RuntimeException("delete me");
    }

    static int[] $SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type()
    {
        $SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type;
        if($SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type == null) goto _L2; else goto _L1
_L1:
        return;
_L2:
        JVM INSTR pop ;
        int ai[] = new int[Type.values().length];
        try
        {
            ai[Type.TAG_Byte.ordinal()] = 2;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Byte_Array.ordinal()] = 8;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Compound.ordinal()] = 11;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Double.ordinal()] = 7;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_End.ordinal()] = 1;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Float.ordinal()] = 6;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Int.ordinal()] = 4;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_List.ordinal()] = 10;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Long.ordinal()] = 5;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_Short.ordinal()] = 3;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Type.TAG_String.ordinal()] = 9;
        }
        catch(NoSuchFieldError _ex) { }
        return $SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type = ai;
    }

    private final Type type;
    private Type listType;
    private final String name;
    private Object value;
    private static int $SWITCH_TABLE$com$carneiro$mcredsim$Tag$Type[];
}