import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useAuth } from '../../../components/provider/AuthProvider';
import { environment } from '../../../environments/environment';
import { Box, Button, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography } from '@mui/material';
import { useNotification } from '../../../components/provider/NotificationContext';

interface User {
  id: number;
  email: string;
  phoneNumber: string;
}

const UserList: React.FC = () => {
  const { token } = useAuth();
  const { notify } = useNotification();
  const [users, setUsers] = useState<User[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await axios.get<User[]>(`${environment.apiUrl}Users`, {
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
        });
        setUsers(response.data);
      } catch (err) {
        setError('Failed to fetch users.');
        notify('Failed to fetch users.', 'error');
      }
    };

    fetchUsers();
  }, [token, notify]);

  const handleDelete = async (userId: number) => {
    try {
      await axios.delete(`${environment.apiUrl}Users/${userId}`, {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
      setUsers(users.filter((user) => user.id !== userId));
      notify('User deleted successfully.', 'success');
    } catch (err) {
      setError('Failed to delete user.');
      notify('Failed to delete user.', 'error');
    }
  };

  return (
    <Box>
      <Typography variant="h4" component="h2" align="center" gutterBottom>
        User List
      </Typography>
      {error && <Typography color="error">{error}</Typography>}
      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Phone Number</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user) => (
              <TableRow key={user.id}>
                <TableCell>{user.id}</TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>{user.phoneNumber}</TableCell>
                <TableCell>
                  <Button variant="contained" color="secondary" onClick={() => handleDelete(user.id)}>
                    Delete
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default UserList;
