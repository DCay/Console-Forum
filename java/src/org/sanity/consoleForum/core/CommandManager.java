package org.sanity.consoleForum.core;

import java.io.IOException;
import java.lang.reflect.InvocationTargetException;

public interface CommandManager {
    void reset();

    void handleInput() throws IOException, InterruptedException, ClassNotFoundException, InvocationTargetException, NoSuchMethodException, InstantiationException, IllegalAccessException;
}
