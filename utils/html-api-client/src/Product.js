import { useEffect, useState } from 'react';

function Product() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [items, setItems] = useState([]);

  useEffect(() => {
    fetch("http://localhost:53282/api/products")
      .then(res => res.json())
      .then(
          (result) => {
              setIsLoaded(true);
              setItems(result)
          },
          (error) => {
              setIsLoaded(true);
              setError(error);
          }
      )

  }, [])

  if (error) {
      return <div>Error: {error.Message}</div>;
  } 
  else if (!isLoaded) {
    return <div>Loading...</div>
  } else {
      return (
          <table>
              <thead>
                <tr>
                    <th>Id</th>
                    <th>Name</th>
                    <th>Supplier</th>
                    <th>Category</th>
                    <th>Unit Price</th>
                </tr>
              </thead>
              <tbody>
                {items.map(item => (
                    <tr>
                        <td>{item.id}</td>
                        <td>{item.name}</td>
                        <td>{item.supplier}</td>
                        <td>{item.category}</td>
                        <td>{item.unit_price}</td>
                    </tr>
                ))}
              </tbody>
          </table>
      )
  }
}

export default Product;